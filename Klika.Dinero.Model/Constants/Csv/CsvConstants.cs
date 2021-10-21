using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Klika.Dinero.Model.Constants.Csv
{
    public static class CsvConstants
    {
        public const int MaxFileSizeMb = 20;
        public const long MaxFileSizeBytes = MaxFileSizeMb * ByteSize.MegaByte;
        public static readonly List<char> SupportedDelimiters = new List<char> { ',', ';', '|'};
        public static readonly List<string> SupportedContentTypes = new List<string> {
            "text/comma-separated-values", "text/csv", "application/csv", "application/excel", "application/vnd.ms-excel", "application/vnd.msexcel", "application/octet-stream"};
        public const string FromDate = "01/01/2019";
        public const string OtherCategoryName = "other";
        public const string IncomeCategoryName = "salary";
        public const string DecimalSeparator = ".";
        public const char DefaultSeparator = ',';
        public const char KeywordsSeparator = ';';
        public static readonly Regex UnwantedCharachtersRegex = new Regex("\"|\'");
    }

    public static class CsvExportConstants
    {
        public const string delimiter = ";";
        public const string dateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public const string amountFormat = "0.00";
        public const string csvColumnHeader = "date_of_transaction;designation;amount;bank;account_no;iban";
        public static string fileName(string month, string year) => $"DineroTransactions-{year}-{month}.csv";
    }
    public static class CsvColumnIndex
    {
        public const int DateOfTransaction = 0;
        public const int Designation = 1;
        public const int Amount = 2;
        public const int Bank = 3;
        public const int AccountNumber = 4;
        public const int IBAN = 5;
    }
    
    public static class CsvColumnHeaders
    {
        public const string DateOfTransaction = "date_of_transaction";
        public const string Designation = "designation";
        public const string Amount = "amount";
        public const string Bank = "bank";
        public const string AccountNumber = "account_no";
        public const string IBAN = "iban";

        public static readonly List<string> Labels = new List<string>
        {
            DateOfTransaction, Designation, Amount, Bank, AccountNumber, IBAN
        };
    }
}