using Klika.Dinero.Model.Entities;
using Klika.Dinero.Model.FileItem;

namespace Klika.Dinero.Model.Errors
{
    // Remove descriptions after FE,Mobile and QA finishes debugging
    public static class CsvErrors
    {
        public const string InvalidFieldCount = "InvalidFieldCount";
        public const string UnsupportedDelimiter = "UnsupportedDelimiter";
        public const string InvalidAccountNumber = "InvalidAccountNumber";
        public const string InvalidBank = "InvalidBank";
        public const string InvalidAccountBank = "InvalidAccountBank";
        public const string InvalidIBAN = "InvalidIBAN";
        public const string ParseException = "ParseException";
        public const string InvalidAccountIBAN = "InvalidAccountIBAN";
        public const string InvalidAccountUser = "InvalidAccountUser";
        public const string IBANAlreadyExists = "IBANAlreadyExists";
        public const string AccountBankAlreadyExist = "AccountBankAlreadyExist";
    }
    
    public class CsvParseError : ApiError
    {
        public long LineIndex { get; set; }

        public CsvParseError(string code, string description)
            : base(code, description)
        {
        }
        
        public CsvParseError(string code, TransactionFileitem tfi) 
            : base(code, $@"{tfi.LineIndex}:{tfi.Transaction.Designation}:{tfi.Transaction.Amount}")
        {
            LineIndex = tfi.LineIndex;
        }
        
        public override bool Equals(object? obj)
        {
            if (obj is CsvParseError err)
            {
                return err.LineIndex == LineIndex &&
                       err.Code == this.Code &&
                       err.Description == this.Description;
            }
            
            return false;
        }
    }
}