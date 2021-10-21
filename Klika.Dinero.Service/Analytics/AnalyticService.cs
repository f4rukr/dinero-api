using AutoMapper;
using Klika.Dinero.Database.DbContexts;
using Klika.Dinero.Model.DTO.Analytic.Request;
using Klika.Dinero.Model.DTO.Analytic.Response;
using Klika.Dinero.Model.DTO.TransactionCategory.Response;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Klika.Dinero.Service.Analytics
{
    public class AnalyticService : IAnalyticService
    {
        private readonly DineroDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AnalyticService> _logger;

        public AnalyticService(
            DineroDbContext dbContext,
            IMapper mapper,
            ILogger<AnalyticService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IncomeExpenseResponseDTO> GetIncomeExpenseAsync(AnalyticRequestDTO request)
        {
            try
            {
                IncomeExpenseResponseDTO response = new IncomeExpenseResponseDTO();

                var query = _dbContext.Transactions.AsNoTracking()
                                                   .Where(x => x.Account.UserId == request.UserId
                                                            && x.DateOfTransaction.Month == request.Date.Value.Month
                                                            && x.DateOfTransaction.Year == request.Date.Value.Year);

                if (!string.IsNullOrEmpty(request.AccountNumber))
                    query = query.Where(x => x.Account.AccountNumber == request.AccountNumber);

                if (request.BankIds?.Count > 0)
                    query = query.Where(x => request.BankIds.Contains(x.Account.BankId));

                var incomeExpenseSum = await query.GroupBy(x => x.Amount < 0)
                                                   .Select(x => x.Sum(t => t.Amount))
                                                   .ToListAsync()
                                                   .ConfigureAwait(false);

                if(!(incomeExpenseSum?.Count > 0))
                {
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.TransactionsNotFound));
                    return response;
                }

                if (incomeExpenseSum.Count == 2)
                {
                    response.Income = incomeExpenseSum.ElementAt(0);
                    response.Expense = incomeExpenseSum.ElementAt(1);
                }

                else if (incomeExpenseSum.ElementAt(0) > 0)
                    response.Income = incomeExpenseSum.ElementAt(0);

                else if (incomeExpenseSum.ElementAt(0) < 0)
                    response.Expense = incomeExpenseSum.ElementAt(0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetIncomeExpenseAsync));
                throw;
            }
        }

        public async Task<PieCharListResponse> GetExpenseByCategoriesAsync(AnalyticRequestDTO request)
        {
            try
            {
                PieCharListResponse response = new PieCharListResponse();

                var query = _dbContext.Transactions.AsNoTracking()
                                                   .Where(x => x.Account.UserId == request.UserId
                                                            && x.DateOfTransaction.Month == request.Date.Value.Month
                                                            && x.DateOfTransaction.Year == request.Date.Value.Year
                                                            && x.Amount < 0);

                if (!string.IsNullOrEmpty(request.AccountNumber))
                    query = query.Where(x => x.Account.AccountNumber == request.AccountNumber);

                if (request.BankIds?.Count > 0)
                    query = query.Where(x => request.BankIds.Contains(x.Account.BankId));

                response.PieChars = await query.AsNoTracking()
                                               .GroupBy(x => new TransactionCategoryResponseDTO()
                                               {
                                                   Id = x.TransactionCategory.Id,
                                                   Name = x.TransactionCategory.Name,
                                                   Description = x.TransactionCategory.Description,
                                                   Keywords = x.TransactionCategory.Keywords
                                               })
                                               .Select(x => new PieCharReponseDTO()
                                               {
                                                   TransactionCategory = x.Key,
                                                   Expense = x.Sum(y => y.Amount)
                                               })
                                               .OrderBy(x => x.Expense)
                                               .ToListAsync().ConfigureAwait(false);

                if (!(response.PieChars?.Count > 0))
                {
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.TransactionsNotFound));
                    return response;
                }
                    
                var sum = response.PieChars.Sum(x => x.Expense);

                foreach(var pieChar in response.PieChars)
                    pieChar.ExpensePercentage = Decimal.Round(pieChar.Expense * 100 / sum, 1);

                var diff = 100 - response.PieChars.OrderBy(x => x.ExpensePercentage).Sum(x => x.ExpensePercentage);
                
                if (diff > 0)
                {
                    foreach (var pieChar in response.PieChars)
                    {
                        if (diff == 0)
                            break;
                        pieChar.ExpensePercentage += 0.1m;
                        diff -= 0.1m;
                    }
                }

                if (diff < 0)
                {
                    foreach (var pieChar in response.PieChars)
                    {
                        if (diff == 0)
                            break;
                        pieChar.ExpensePercentage -= 0.1m;
                        diff += 0.1m;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetExpenseByCategoriesAsync));
                throw;
            }
        }
        
        public async Task<ExpenseDailyMonthListResponse> GetExpenseDailyMonthAsync(AnalyticRequestDTO request)
        {
            try
            {
                var response = new ExpenseDailyMonthListResponse();

                var query = _dbContext.Transactions.AsNoTracking()
                                                   .Where(x => x.Account.UserId == request.UserId 
                                                            && x.DateOfTransaction.Date.Month == request.Date.Value.Month 
                                                            && x.DateOfTransaction.Date.Year == request.Date.Value.Year
                                                            && x.Amount < 0);
                
                if (!string.IsNullOrEmpty(request.AccountNumber))
                    query = query.Where(x => x.Account.AccountNumber == request.AccountNumber);

                if (request.BankIds?.Count > 0)
                    query = query.Where(x => request.BankIds.Contains(x.Account.BankId));

                response.Expenses = await query.GroupBy(t => t.DateOfTransaction.Date)
                                               .OrderBy(x => x.Key)
                                               .Select(g => new ExpenseDailyDTO(g.Key, g.Sum(x => x.Amount)))
                                               .ToListAsync()
                                               .ConfigureAwait(false);

                if (!(response.Expenses?.Count > 0))
                {
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.TransactionsNotFound));
                    return response;
                }

                return response;
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(GetExpenseDailyMonthAsync));
                throw;
            }
        }
        
        public async Task<ExpenseDailyListResponse> GetExpenseDailyAsync(AnalyticRequestDTO request)
        {
            try
            {
                var response = new ExpenseDailyListResponse();
                
                var query = _dbContext.Transactions.AsNoTracking()
                                                   .Where(x => x.Account.UserId == request.UserId
                                                            && x.DateOfTransaction.Date == request.Date.Value.Date
                                                            && x.Amount < 0);
                
                if (!string.IsNullOrEmpty(request.AccountNumber))
                    query = query.Where(x => x.Account.AccountNumber == request.AccountNumber);

                if (request.BankIds?.Count > 0)
                    query = query.Where(x => request.BankIds.Contains(x.Account.BankId));

                response.Expenses = await query.Select(x => new TransactionDTO
                                               {
                                                   Designation = x.Designation,
                                                   Amount = x.Amount,
                                                   DateOfTransaction = x.DateOfTransaction,
                                                   Bank = x.Account.Bank.Name,
                                                   Category = x.TransactionCategory.Name
                                               })
                                               .OrderByDescending(x => x.DateOfTransaction)
                                               .ToListAsync()
                                               .ConfigureAwait(false);

                if (!(response.Expenses?.Count > 0))
                {
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.TransactionsNotFound));
                    return response;
                }

                return response;
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(GetExpenseDailyAsync));
                throw;
            }
        }
    }
}
