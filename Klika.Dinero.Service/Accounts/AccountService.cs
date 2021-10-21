using AutoMapper;
using Klika.Dinero.Database.DbContexts;
using Klika.Dinero.Model.Entities;
using Klika.Dinero.Model.Interfaces;
using Klika.Dinero.Model.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.DTO.Account.Response;
using Klika.Dinero.Model.DTO.Account.Request;
using Klika.Dinero.Model.DTO.Bank.Response;

namespace Klika.Dinero.Service.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly DineroDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            DineroDbContext dbContext,
            IMapper mapper,
            ILogger<AccountService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AccountResponseDTO> CreateAccountAsync(AccountRequestDTO request)
        {
            try
            {
                AccountResponseDTO response = new AccountResponseDTO();
                
                Bank bank = await _dbContext.Banks.AsNoTracking()
                                                  .FirstOrDefaultAsync(x => x.Id == request.BankId)
                                                  .ConfigureAwait(false);

                if (bank == null)
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.BankNotFound));

                if (!Enum.IsDefined(typeof(CurrencyCode), request.CurrencyCode))
                    response.Errors.Add(new ApiError(ErrorCodes.InvalidFormat, ErrorDescriptions.CurrencyCodeInvalidFormat));

                Account check = await _dbContext.Accounts.AsNoTracking()
                                                         .FirstOrDefaultAsync(x => x.AccountNumber == request.AccountNumber)
                                                         .ConfigureAwait(false);
                
                if (check != null)
                    response.Errors.Add(new ApiError(ErrorCodes.AlreadyExist, ErrorDescriptions.AccountNumberAlreadyExist));

                check = await _dbContext.Accounts.AsNoTracking()
                                                 .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.BankId == request.BankId)
                                                 .ConfigureAwait(false);

                if (check != null)
                    response.Errors.Add(new ApiError(ErrorCodes.AlreadyExist, ErrorDescriptions.AccountBankAlreadyExist));

                if (request.IBAN != null)
                {
                    check = await _dbContext.Accounts.AsNoTracking()
                                                     .FirstOrDefaultAsync(x => x.IBAN == request.IBAN)
                                                     .ConfigureAwait(false);

                    if (check != null)
                        response.Errors.Add(new ApiError(ErrorCodes.AlreadyExist, ErrorDescriptions.AccountIBANAlreadyExist));
                }

                if (!response.Succeeded)
                    return response;

                Account account = _mapper.Map<AccountRequestDTO, Account>(request);
                
                await _dbContext.AddAsync(account).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                account.Bank = bank;
                response = _mapper.Map<AccountResponseDTO>(account);
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateAccountAsync));
                throw;
            }
        }

        public async Task<AccountResponseDTO> GetAccountAsync(string accountNumber, string userId)
        {
            try
            {
                AccountResponseDTO response = await _dbContext.Accounts.AsNoTracking()
                                                                        .Where(x => x.AccountNumber == accountNumber && x.UserId == userId)
                                                                        .Select(x => new AccountResponseDTO()
                                                                        {
                                                                            AccountNumber = x.AccountNumber,
                                                                            IBAN = x.IBAN,
                                                                            Bank = new BankResponseDTO()
                                                                            {
                                                                                Id = x.Bank.Id,
                                                                                Name = x.Bank.Name,
                                                                                Description = x.Bank.Description
                                                                            },
                                                                            DateOfLastTransaction = x.Transactions.Max(t => t.DateOfTransaction),
                                                                            CurrencyCode = x.CurrencyCode,
                                                                            Balance = x.Transactions.Sum(t => t.Amount)
                                                                        })
                                                                        .FirstOrDefaultAsync()
                                                                        .ConfigureAwait(false);

                if (response == null)
                {
                    response = new AccountResponseDTO();
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.AccountNotFound));
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAccountAsync));
                throw;
            }
        }
        
        public async Task<AccountListResponse> GetAccountsAsync(string userId)
        {
            try
            {
                AccountListResponse response = new AccountListResponse();
                
                response.Accounts = await _dbContext.Accounts.AsNoTracking()
                                                             .Where(x => x.UserId == userId)
                                                             .Select(x => new AccountResponseDTO()
                                                             {
                                                                 AccountNumber = x.AccountNumber,
                                                                 IBAN = x.IBAN,
                                                                 Bank = new BankResponseDTO()
                                                                 {
                                                                     Id = x.Bank.Id,
                                                                     Name = x.Bank.Name,
                                                                     Description = x.Bank.Description
                                                                 },
                                                                 DateOfLastTransaction = x.Transactions.Max(t => t.DateOfTransaction),
                                                                 CurrencyCode = x.CurrencyCode,
                                                                 Balance = x.Transactions.Sum(t => t.Amount)
                                                             })
                                                             .OrderBy(x => x.Bank.Name)
                                                             .ToListAsync()
                                                             .ConfigureAwait(false);
                
                if (!(response.Accounts?.Count > 0))
                {
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.AccountsNotFound));
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAccountsAsync));
                throw;
            }
        }
        
        public async Task<ApiResponse> DeleteAccountAsync(string accountNumber, string userId)
        {
            try
            {
                ApiResponse response = new ApiResponse();

                Account account = await _dbContext.Accounts.AsNoTracking()
                                                           .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber && x.UserId == userId)
                                                           .ConfigureAwait(false);

                if(account == null)
                {
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.AccountNotFound));
                    return response;
                }

                _dbContext.Accounts.Remove(account);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(DeleteAccountAsync));
                throw;
            }
        }
    }
}
