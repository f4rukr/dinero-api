using System;
using System.Collections.Generic;

namespace Klika.Dinero.Model.Errors
{
    public static class ErrorDescriptions
    {
        public const string MissingFile = "MissingFile";
        public const string EmptyFile = "File must not be empty.";
        public const string NoTransactions = "NoTransactions";
        public const string BankNotFound = "Bank with given Id not found.";
        public const string AccountWithBankNotFound = "Account with given bank not found.";
        public const string AccountNotFound = "Account with given accountNumber not found.";
        public const string AccountsNotFound = "Accounts not found.";
        public const string TransactionsNotFound = "No transactions found for given parameters.";
        public const string CurrencyCodeInvalidFormat = "Currency code is not supported.";
        public const string RequestParameterInvalidFormat = "Request parameter could not be parsed.";
        public const string AccountNumberAlreadyExist = "Account with given accountNumber already exists.";
        public const string AccountBankAlreadyExist = "Account with given bankId already exists.";
        public const string MissingCsvHeaders = "Headers could not be parsed. Check if they are valid and not missing.";
        public const string AccountIBANAlreadyExist = "Account with given IBAN already exists.";
        public const string InvalidDesignation = "Designation length is invalid or it contains unsupported charachters";
        public const string InvalidAmount = "Amount is <= 0";
        public static string PayloadTooLarge(long max) => $"Maximum payload size is {max} bytes";        
        public static string UnsupportedContentType(List<string> supportedTypes) => 
            $"Supported content types are: {String.Join(", ", supportedTypes)}";
        public static string UnsupportedFileType(string supportedType) => $"Supported file type is {supportedType}";
        public const string InvalidIncomeCategory = "InvalidIncomeCategory";
        public const string InvalidOtherCategory =  "InvalidOtherCategory";
        public static string UnsupportedFileType(List<string> supportedTypes) =>  $"Supported file types are: [{String.Join(',', supportedTypes)}";
        public const string SqlInsertFailed = "Failed to insert row";
        public const string CsvInsertError = "Failed to insert CSV into the database. Check if your dates for given accounts are unique";
        public const string NumberOfElementsRequired = "Number of elements is required";
        public const string CurrentIndexRequired = "Current index is required";
        public const string InvalidDate = "Date is invalid";
        public const string InvalidRange = "Invalid range";
        public const string InvalidCsvHeaders = "CSV File headers are invalid or in wrong order";
        public static string InvalidFieldCount(int validCount) => $"Invalid field count. Valid count is {validCount}";
        public static string SentOnEmail(string email) => $"Errors are sent to {email}";
    }
}