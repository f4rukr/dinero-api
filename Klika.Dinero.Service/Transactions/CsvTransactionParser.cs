using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Klika.Dinero.Database.DbContexts;
using Klika.Dinero.Model.Interfaces;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Klika.Dinero.Model.Constants.RegexConstants;
using Klika.Dinero.Model.DTO.Transaction.Response;
using Klika.Dinero.Model.Entities;
using Klika.Dinero.Model.FileItem;
using Klika.Dinero.Model.Helpers.CsvParser;
using Microsoft.EntityFrameworkCore;
using Klika.Dinero.Model.Constants.Csv;

namespace Klika.Dinero.Service.Transactions
{
    /// <summary>
    /// CsvTransactionParser class is used to parse CSV Transactions into TransactionFileItems,
    /// Delimiter can be set manually or determined automatically.
    /// </summary>
    public class CsvTransactionParser : ICsvTransactionParser
    {
        private readonly ILogger<CsvTransactionParser> _logger;
        private readonly DineroDbContext _dbContext;
        private char _delimiter;
        
        public CsvTransactionParser(ILogger<CsvTransactionParser> logger, DineroDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _delimiter = CsvConstants.DefaultSeparator;
        }

        public static char DeterminDelimiter(string line) => CsvConstants.SupportedDelimiters.FirstOrDefault(line.Contains);

        public async Task<string> ReadHeadersAsync(IFormFile file)
        {
            string headers = null;
            
            using (var sr = new StreamReader(file.OpenReadStream()))
            {
                headers = await sr.ReadLineAsync().ConfigureAwait(false);
            }

            return headers;
        }
        
        public async Task<List<string>> ReadLinesAsync(IFormFile file)
        {
            var fileLines = new List<string>();
            
            using (var sr = new StreamReader(file.OpenReadStream()))
            {
                while (sr.Peek() >= 0)
                {
                    var line = await sr.ReadLineAsync().ConfigureAwait(false);
                   
                    if(!string.IsNullOrWhiteSpace(line))
                        fileLines.Add(line.Replace(CsvConstants.UnwantedCharachtersRegex, ""));
                }
            }

            return fileLines;
        }

        public async Task<TransactionsParseResponse> ParseAndValidateCSVAsync(IFormFile file)
        {
            var lines = await this.ReadLinesAsync(file).ConfigureAwait(false);
            
            _delimiter = CsvTransactionParser.DeterminDelimiter(lines[0]);
            if (_delimiter == default(char))
                return new TransactionsParseResponse(new CsvParseError(ErrorCodes.InvalidFormat, CsvErrors.UnsupportedDelimiter));
            
            var _transactionCategories = await _dbContext.TransactionCategories.AsNoTracking()
                                                                               .Select(x => new Category(x.Id, x.Name, x.Keywords))
                                                                               .ToListAsync()
                                                                               .ConfigureAwait(false);

            TransactionCategoryMap transactionCategoryMap = null;
            try
            {
                transactionCategoryMap = new TransactionCategoryMap(_transactionCategories);
            }
            catch (InvalidCategoryConfiguration ex)
            {
                _logger.LogError(ex, nameof(CsvTransactionParser));
                return new TransactionsParseResponse(new CsvParseError(ex.Code, ex.Description));
            }
            
            var bankDictionary = await _dbContext.Banks.AsNoTracking()
                                                    .ToDictionaryAsync(t => t.Id, t => t.Name)
                                                    .ConfigureAwait(false);

            var parseErrors = new ConcurrentBag<CsvParseError>();
            var parsedTransactions = new ConcurrentBag<TransactionFileitem>();

            Parallel.ForEach(lines.Skip(1), (line, _, index) =>
            {
                var parseResponse = this.FromCsv(line, index);
                var tfi = parseResponse.TransactionFileitem;
                
                tfi.BankId = bankDictionary
                    .FirstOrDefault(x => String.Equals(x.Value, tfi.BankName, StringComparison.CurrentCultureIgnoreCase)).Key;
                
                if (tfi.BankId == 0)
                    parseResponse.Errors.Add(new CsvParseError(CsvErrors.InvalidBank, tfi));
                
                tfi.Transaction.TransactionCategoryId = transactionCategoryMap.FindCategoryKey(tfi.Transaction.Designation);

                tfi.IsExpense = tfi.Transaction.TransactionCategoryId != transactionCategoryMap.IncomeCategoryId;

                if(parseErrors.IsEmpty)
                    parsedTransactions.Add(parseResponse.TransactionFileitem);
                
                foreach(var error in parseResponse.Errors)
                    parseErrors.Add(error);
            });

            if (!(parseErrors.IsEmpty))
                return new TransactionsParseResponse(parseErrors.OrderBy(x => x.LineIndex).ToList());

            return new TransactionsParseResponse(parsedTransactions.OrderBy(x => x.LineIndex).ToList());
        }
        
        public TransactionLineParseResponse FromCsv(string csvLine, long index)
        {
            string[] values = csvLine.Split(_delimiter);
            
            if (values.Length != CsvColumnHeaders.Labels.Count)
                return new TransactionLineParseResponse(
                    new CsvParseError(CsvErrors.InvalidFieldCount, ErrorDescriptions.InvalidFieldCount(CsvColumnHeaders.Labels.Count)));
            
            var response = new TransactionLineParseResponse
            {
                TransactionFileitem = new TransactionFileitem
                {
                    Transaction = new Transaction()
                }
            };
            
            var parsedTransaction = response.TransactionFileitem.Transaction;
            
            try
            {
                response.TransactionFileitem.LineIndex = ++index;
                parsedTransaction.Designation = Convert.ToString(values[CsvColumnIndex.Designation].Trim());;
               
                try
                {
                    parsedTransaction.Amount = Convert.ToDecimal(values[CsvColumnIndex.Amount], 
                        new NumberFormatInfo { NumberDecimalSeparator = CsvConstants.DecimalSeparator });
                    if (parsedTransaction.Amount <= 0m)
                        response.Errors.Add(new CsvParseError(TransactionErrorCodes.InvalidAmount, response.TransactionFileitem));
                }
                catch (FormatException ex)
                {
                    _logger.LogError(ex, nameof(CsvTransactionParser));
                    response.Errors.Add(new CsvParseError(TransactionErrorCodes.InvalidAmount, response.TransactionFileitem));
                }
            
                if (!(RegexValidators.IsValidDesignation(parsedTransaction.Designation)))
                    response.Errors.Add(new CsvParseError(TransactionErrorCodes.InvalidDesignation, response.TransactionFileitem));

                try
                {
                    parsedTransaction.DateOfTransaction = Convert.ToDateTime(values[CsvColumnIndex.DateOfTransaction]);
                    if (parsedTransaction.DateOfTransaction <= DateTime.Parse(CsvConstants.FromDate) 
                        ||  parsedTransaction.DateOfTransaction > DateTime.Now)
                        response.Errors.Add(new CsvParseError(TransactionErrorCodes.InvalidDate, response.TransactionFileitem));
                }
                catch (FormatException ex)
                {
                    _logger.LogError(ex, nameof(CsvTransactionParser));
                    response.Errors.Add(new CsvParseError(TransactionErrorCodes.InvalidDate, response.TransactionFileitem));
                }
        
                response.TransactionFileitem.BankName = Convert.ToString(values[CsvColumnIndex.Bank].Trim());
    
                response.TransactionFileitem.AccountNumber = Convert.ToString(values[CsvColumnIndex.AccountNumber].Trim());
                if (!(RegexValidators.IsValidAccountNumber(response.TransactionFileitem.AccountNumber)))
                    response.Errors.Add(new CsvParseError(CsvErrors.InvalidAccountNumber, response.TransactionFileitem));

                response.TransactionFileitem.IBAN = Convert.ToString(values[CsvColumnIndex.IBAN].Trim());
                if (response.TransactionFileitem.IBAN != null && !(RegexValidators.IsValidIBAN(response.TransactionFileitem.IBAN)))
                    response.Errors.Add(new CsvParseError(CsvErrors.InvalidIBAN, response.TransactionFileitem));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CsvTransactionParser));
                response.Errors.Add(new CsvParseError(CsvErrors.ParseException, response.TransactionFileitem));
            }

            return response;
        }
    }
}