using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Klika.Dinero.Model.Errors;
using Microsoft.AspNetCore.Http;
using Klika.Dinero.Model.Entities;
using Klika.Dinero.Model.Response;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Klika.Dinero.Model.Interfaces;
using Klika.Dinero.Database.DbContexts;
using Klika.Dinero.Model.DTO.TransactionCategory.Response;
using System.Collections.Generic;
using Klika.Dinero.Model.Extensions;
using System.IO;
using System.Transactions;
using Transaction = Klika.Dinero.Model.Entities.Transaction;
using Klika.Dinero.Model.DTO.Transaction.Response;
using Klika.Dinero.Model.DTO.Transaction.Request;
using Klika.Dinero.Model.DTO.Analytic.Response;
using Klika.Dinero.Model.Helpers.Bulk;
using Klika.Dinero.Model.Helpers.Pagination;
using Klika.Dinero.Model.Constants.Email;
using Microsoft.FeatureManagement;
using Klika.Dinero.Model.Constants.FeatureFlags;
using Klika.Dinero.Model.Constants.Csv;
using Klika.Dinero.Model.Constants.RegexConstants;
using Klika.Dinero.Model.Helpers.CsvParser;

namespace Klika.Dinero.Service.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly DineroDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly ICsvTransactionParser _transactionParser;
        private readonly IBulkService _bulkService;
        private readonly IMailService _mailService;
        private readonly IFileGeneratorService _fileGeneratorService;
        private readonly IFeatureManager _featureManager;

        public TransactionService(
            IMapper mapper,
            ILogger<TransactionService> logger,
            DineroDbContext dbContext,
            ICsvTransactionParser transactionParser,
            IBulkService bulkService,
            IMailService mailService,
            IFileGeneratorService fileGeneratorService, 
            IFeatureManager featureManager)
        {
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
            _transactionParser = transactionParser;
            _bulkService = bulkService;
            _mailService = mailService;
            _fileGeneratorService = fileGeneratorService;
            _featureManager = featureManager;
        }

        public async Task<ApiResponse> ValidateCsvTransactionsFileAsync(IFormFile file)
        {
            var response = new ApiResponse();

            if (file is null)
            {
                response.Errors.Add(new ApiError(ErrorCodes.MissingParameter, ErrorDescriptions.MissingFile));
                return response;
            }
            
            if (!CsvConstants.SupportedContentTypes.Contains(file.ContentType))
                response.Errors.Add(new ApiError(ErrorCodes.UnsupportedContentType, ErrorDescriptions.UnsupportedContentType(CsvConstants.SupportedContentTypes)));
            
            if(Path.GetExtension(file.FileName) != ".csv")    
                response.Errors.Add(new ApiError(ErrorCodes.UnsupportedExtension, ErrorDescriptions.UnsupportedFileType(".csv")));

            if (file.Length == 0)
                response.Errors.Add(new ApiError(ErrorCodes.EmptyPayload, ErrorDescriptions.EmptyFile));
            
            if (file.Length > CsvConstants.MaxFileSizeBytes)
                response.Errors.Add(new ApiError(ErrorCodes.PayloadTooLarge, ErrorDescriptions.PayloadTooLarge(CsvConstants.MaxFileSizeBytes)));

            var headers = await _transactionParser.ReadHeadersAsync(file).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(headers)) {
                response.Errors.Add(new ApiError(ErrorCodes.MissingCsvHeaders, ErrorDescriptions.MissingCsvHeaders));
                return response;
            }
            
            var parsedHeaders = headers.Trim()
                                       .Replace(CsvConstants.UnwantedCharachtersRegex, string.Empty)
                                       .Split(CsvTransactionParser.DeterminDelimiter(headers));
        
            if(!CsvColumnHeaders.Labels.SequenceEqual(parsedHeaders, StringComparer.OrdinalIgnoreCase))
                response.Errors.Add(new ApiError(ErrorCodes.InvalidCsvHeaders, ErrorDescriptions.InvalidCsvHeaders));
                
            return response;
        }
        
        public async Task<TransactionUploadResponse> ParseCsvTransactionsAsync(TransactionUploadRequestDTO dto)
        {
            try
            {
                var sendErrorsViaEmailFeature = await _featureManager.IsEnabledAsync(FeatureFlags.SendErrorsViaEmail);

                var parseResponse = await _transactionParser.ParseAndValidateCSVAsync(dto.File).ConfigureAwait(false);

                if (parseResponse.ParsedTransactions.Count == 0 && parseResponse.ParseErrors.Count == 0)
                    return new TransactionUploadResponse(new ApiError(ErrorCodes.EmptyPayload, ErrorDescriptions.NoTransactions));

                if (parseResponse.ParseErrors.Count != 0)
                {
                    if(parseResponse.ParseErrors.Count > 1 && sendErrorsViaEmailFeature)
                    {

                        var excelAttachment = _fileGeneratorService.GenerateErrorsExcelDocument(parseResponse.ParseErrors);
                        await _mailService.SendEmailAsync(dto.Email, EmailConstants.FailedCsvParsing, RawEmailTemplates.GetErrorListMessageBody(excelAttachment)).ConfigureAwait(false);
                        
                        return new TransactionUploadResponse(new List<ApiError> {
                            new ApiError(ErrorCodes.SentOnEmail, ErrorDescriptions.SentOnEmail(dto.Email))
                            });
                    }                        

                    return new TransactionUploadResponse(new List<ApiError>(parseResponse.ParseErrors));
                }
                   

                var mostRecentTransaction = parseResponse.ParsedTransactions.Max(x => x.Transaction.DateOfTransaction);
                var response = new TransactionUploadResponse(mostRecentTransaction);
                
                var parsedAccountNumbers = parseResponse.ParsedTransactions.Select(x => x.AccountNumber).Distinct().ToList();
                var parsedIBANs = parseResponse.ParsedTransactions.Select(x => x.IBAN).Distinct().ToList();
                var existingAccounts = await _dbContext.Accounts.Where(x => parsedAccountNumbers.Contains(x.AccountNumber) ||
                                                                             parsedIBANs.Contains(x.IBAN) ||
                                                                             x.UserId == dto.UserId)
                                                                .ToListAsync()
                                                                .ConfigureAwait(false);
                
                var accounts = new List<Account>();
                var transactions = new List<Transaction>(capacity: parseResponse.ParsedTransactions.Count);
                
                foreach (var parsedTransaction in parseResponse.ParsedTransactions)
                {
                    var account = existingAccounts.FirstOrDefault(x => x.AccountNumber == parsedTransaction.AccountNumber &&
                                                                       x.IBAN == parsedTransaction.IBAN);
             
                    if (account == null)
                    {
                        if (parsedTransaction.IBAN != null && existingAccounts.FirstOrDefault(x => x.IBAN == parsedTransaction.IBAN) is not null)
                        {
                            response.Errors.Add(new CsvParseError(CsvErrors.IBANAlreadyExists, parsedTransaction));
                            continue;
                        }
                    
                        if (existingAccounts.FirstOrDefault(x => x.BankId == parsedTransaction.BankId) is not null)
                        {
                            response.Errors.Add(new CsvParseError(CsvErrors.AccountBankAlreadyExist, parsedTransaction));
                            continue;
                        }
                        
                        account = new Account(parsedTransaction.BankId, parsedTransaction.AccountNumber, parsedTransaction.IBAN, dto.UserId);
                        accounts.Add(account);
                        existingAccounts.Add(account);
                    }
                    else
                    {
                        bool isError = false;
                        
                        if (account.UserId != dto.UserId)
                        {
                            response.Errors.Add(new CsvParseError(CsvErrors.InvalidAccountUser, parsedTransaction));
                            isError = true;
                        }
                    
                        if (account.BankId != parsedTransaction.BankId)
                        {
                            response.Errors.Add(new CsvParseError(CsvErrors.InvalidAccountBank, parsedTransaction));
                            isError = true;
                        }
                    
                        if (!string.IsNullOrEmpty(parsedTransaction.IBAN))
                        {
                            if (account.IBAN == null)
                                account.IBAN = parsedTransaction.IBAN;
                            
                            else if(account.IBAN != parsedTransaction.IBAN)
                            {
                                response.Errors.Add(new CsvParseError(CsvErrors.InvalidAccountIBAN, parsedTransaction));
                                isError = true;
                            }
                        }
                            
                        if (isError)
                            continue;
                    }

                    if (parsedTransaction.IsExpense)
                        parsedTransaction.Transaction.Amount *= -1;
                    
                    parsedTransaction.Transaction.Account = account;
                    transactions.Add(parsedTransaction.Transaction);
                }

                if (!response.Succeeded) {
                    if (response.Errors.Count > 1 && sendErrorsViaEmailFeature)
                    {
                        var excelAttachment = _fileGeneratorService.GenerateErrorsExcelDocument(response.Errors);
                        await _mailService.SendEmailAsync(dto.Email, EmailConstants.FailedCsvParsing, RawEmailTemplates.GetErrorListMessageBody(excelAttachment)).ConfigureAwait(false);

                        return new TransactionUploadResponse(new List<ApiError> {
                            new ApiError(ErrorCodes.SentOnEmail, ErrorDescriptions.SentOnEmail(dto.Email))
                            });
                    }                        

                    return response;
                } 

                try
                {
                    using (TransactionScope scope = new(scopeOption: TransactionScopeOption.Required, transactionOptions: new TransactionOptions()
                            { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TimeSpan.Zero },
                        asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await _dbContext.AddRangeAsync(accounts).ConfigureAwait(false);
                        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                        using (TransactionScope bulkScope = new(scopeOption: TransactionScopeOption.Suppress, transactionOptions: new TransactionOptions()
                            { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = TimeSpan.Zero },
                            asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled))
                        {

                            BulkInsertTransactions config = new(transactions)
                            {
                                BatchSize = 5000,
                                ConnectionString = _dbContext.Database.GetConnectionString(),
                                DestinationTable = "Transactions"
                            };

                            await _bulkService.BulkInsertAsync(config).ConfigureAwait(false);
                            bulkScope.Complete();
                        }
                        
                        scope.Complete();
                    }
                }
                catch(Exception ex)
                {
                    response.Errors.Add(new ApiError(ErrorCodes.CsvInsertError, ErrorDescriptions.CsvInsertError));
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ParseCsvTransactionsAsync));
                throw;
            }
        }
        
        public async Task<TransactionCategoryListResponse> GetTransactionCategoriesAsync()
        {
            try
            {
                TransactionCategoryListResponse response = new TransactionCategoryListResponse();

                List<TransactionCategory> transactionCategories = await _dbContext.TransactionCategories.AsNoTracking()
                                                                                                        .ToListAsync()
                                                                                                        .ConfigureAwait(false);

                response.TransactionCategories = _mapper.Map<List<TransactionCategory>, List<TransactionCategoryResponseDTO>>(transactionCategories);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetTransactionCategoriesAsync));
                throw;
            }
        }

        public async Task<TransactionResponseDTO> CreateTransactionAsync(TransactionRequestDTO dto)
        {
            try
            {

                var bank = await _dbContext.Banks.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == dto.BankId)
                    .ConfigureAwait(false);

                if (bank is null)
                    return new TransactionResponseDTO(new ApiError(CsvErrors.InvalidBank, ErrorDescriptions.BankNotFound));

                var transactionWithSameDate = await _dbContext.Transactions.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Account.UserId == dto.UserId &&
                                              x.Account.Bank.Id == dto.BankId &&
                                              x.DateOfTransaction == dto.DateOfTransaction)
                    .ConfigureAwait(false);

                if (transactionWithSameDate is not null)
                    return new TransactionResponseDTO(new ApiError(TransactionErrorCodes.InvalidDate, ErrorDescriptions.InvalidDate));


                if (dto.DateOfTransaction <= DateTime.Parse(CsvConstants.FromDate) || dto.DateOfTransaction > DateTime.Now)
                    return new TransactionResponseDTO(new ApiError(TransactionErrorCodes.InvalidDate, ErrorDescriptions.InvalidDate));

                if (!(RegexValidators.IsValidDesignation(dto.Designation)))
                    return new TransactionResponseDTO(new ApiError(TransactionErrorCodes.InvalidDesignation,
                        ErrorDescriptions.InvalidDesignation));

                if (dto.Amount <= 0)
                    return new TransactionResponseDTO(new ApiError(TransactionErrorCodes.InvalidAmount, ErrorDescriptions.InvalidAmount));

                var usersBankAccount = await _dbContext.Accounts.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.Bank.Id == dto.BankId)
                    .ConfigureAwait(false);

                if (usersBankAccount is null)
                    return new TransactionResponseDTO(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.AccountWithBankNotFound));

                var transactionCategories = await _dbContext.TransactionCategories.AsNoTracking()
                    .Select(x => new Category(x.Id, x.Name, x.Keywords))
                    .ToListAsync()
                    .ConfigureAwait(false);

                var transactionCategoryMap = new TransactionCategoryMap(transactionCategories);
                var transactionCategoryId = transactionCategoryMap.FindCategoryKey(dto.Designation);

                var transaction = new Transaction
                {
                    AccountId = usersBankAccount.Id,
                    TransactionCategoryId = transactionCategoryId,
                    Amount = dto.Amount * (transactionCategoryId == transactionCategoryMap.IncomeCategoryId ? 1 : -1),
                    Designation = dto.Designation,
                    DateOfTransaction = dto.DateOfTransaction
                };

                await _dbContext.Transactions.AddAsync(transaction);
                await _dbContext.SaveChangesAsync();

                return new TransactionResponseDTO(transaction);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(CreateTransactionAsync));
                throw;
            }
    }
        
        public async Task<TransactionsResponseDTO> GetTransactionsByFiltersAsync(TransactionParametersRequestDTO dto)
        {
            try
            {
                var result = _dbContext.Transactions.AsNoTracking()
                                                    .Include(t => t.Account)
                                                    .ThenInclude(t => t.Bank)
                                                    .Where(t => t.Account.UserId == dto.UserId);

                if(dto.Date != null)
                    result = result.Where(t => t.DateOfTransaction.Year == dto.Date.Value.Year &&
                    t.DateOfTransaction.Month == dto.Date.Value.Month);

                if (dto.AccountNumber != null)
                    result = result.Where(t => t.Account.AccountNumber == dto.AccountNumber);

                if (dto.Categories != null)
                    result = result.Where(t => dto.Categories.Contains(t.TransactionCategory.Id));

                if (dto.BankId != null)
                    result = result.Where(t => t.Account.Bank.Id == dto.BankId);

                var orderedResult = result.OrderByDescending(t => t.DateOfTransaction)
                                          .Select(t => new TransactionDTO
                                          {
                                              Amount = t.Amount,
                                              Bank = t.Account.Bank.Name,
                                              Category = t.TransactionCategory.Name,
                                              DateOfTransaction = t.DateOfTransaction,
                                              Designation = t.Designation
                                          });

                var list = await LoadMoreList<TransactionDTO>.ToLoadMoreListAsync(orderedResult, dto.CurrentIndex.Value, dto.NumberOfElements.Value)
                                                             .ConfigureAwait(false);

                TransactionsResponseDTO response = new()
                {
                    Metadata = new LoadMoreMeta
                    {
                        CurrentLimit = list.Limit,
                        CurrentOffset = list.Offset,
                        HasNext = ((list.Offset * list.Limit) + list.Limit) - 1 < list.TotalItems,
                        TotalItems = list.TotalItems
                    },
                    Transactions = list
                };

                if (response.Transactions.Count == 0)
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.TransactionsNotFound));

                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(GetTransactionsByFiltersAsync));
                throw;
            }            
        }

        public async Task<TransactionCSVExportResponse> GetTransactionsForExportAsync(TransactionCSVExportRequestDTO request)
        {
            try
            {
                TransactionCSVExportResponse response = new TransactionCSVExportResponse();

                var query = _dbContext.Transactions.AsNoTracking()
                                                    .Include(t => t.Account)
                                                    .ThenInclude(t => t.Bank)
                                                    .Where(t => t.Account.UserId == request.UserId
                                                             && t.DateOfTransaction.Month == request.Date.Value.Month
                                                             && t.DateOfTransaction.Year == request.Date.Value.Year);

                if (!string.IsNullOrEmpty(request.AccountNumber))
                    query = query.Where(x => x.Account.AccountNumber == request.AccountNumber);

                var transactions = await query.OrderBy(t => t.DateOfTransaction)
                                              .Select(t => new TransactionCSVExportItemDTO
                                              {
                                                  Amount = t.Amount,
                                                  Bank = t.Account.Bank.Name,
                                                  DateOfTransaction = t.DateOfTransaction,
                                                  Designation = t.Designation,
                                                  AccountNumber = t.Account.AccountNumber,
                                                  IBAN = t.Account.IBAN
                                              })
                                              .ToListAsync()
                                              .ConfigureAwait(false);

                if (!(transactions?.Count > 0))
                {
                    response.Errors.Add(new ApiError(ErrorCodes.NotFound, ErrorDescriptions.TransactionsNotFound));
                    return response;
                }

                var fileName = CsvExportConstants.fileName(request.Date.Value.Month.ToString(), request.Date.Value.Year.ToString());

                response.File = new TransactionCSVExport(transactions, fileName);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetTransactionsForExportAsync));
                throw;
            }
        }

        public async Task ExportTransactionsToMailAsync(TransactionCSVExportRequestDTO request)
        {
            try
            {
                var query = _dbContext.Transactions.AsNoTracking()
                                                    .Include(t => t.Account)
                                                    .ThenInclude(t => t.Bank)
                                                    .Where(t => t.Account.UserId == request.UserId
                                                             && t.DateOfTransaction.Month == request.Date.Value.Month
                                                             && t.DateOfTransaction.Year == request.Date.Value.Year);

                if (!string.IsNullOrEmpty(request.AccountNumber))
                    query = query.Where(x => x.Account.AccountNumber == request.AccountNumber);

                var transactions = await query.OrderBy(t => t.DateOfTransaction)
                                              .Select(t => new TransactionCSVExportItemDTO
                                              {
                                                  Amount = t.Amount,
                                                  Bank = t.Account.Bank.Name,
                                                  DateOfTransaction = t.DateOfTransaction,
                                                  Designation = t.Designation,
                                                  AccountNumber = t.Account.AccountNumber,
                                                  IBAN = t.Account.IBAN
                                              })
                                              .ToListAsync()
                                              .ConfigureAwait(false);

                if (!(transactions?.Count > 0))
                {
                    await _mailService.SendEmailAsync(request.Email, EmailConstants.DineroTransactions, RawEmailTemplates.GetNoTransactionsMessageBody()).ConfigureAwait(false);
                }
                else
                {
                    var excelAttachment = _fileGeneratorService.GenerateTransactionsExcelDocument(transactions);
                    await _mailService.SendEmailAsync(request.Email, EmailConstants.DineroTransactions, RawEmailTemplates.GetTransactionsMessageBody(excelAttachment, request.Date.Value)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ExportTransactionsToMailAsync));
                throw;
            }
        }
    }
}