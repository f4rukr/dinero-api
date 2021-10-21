using System.Text.RegularExpressions;

namespace Klika.Dinero.Model.Constants.RegexConstants
{
    public static class RegexValidators
    {
        public const string AccountNumberPattern = "^[0-9]{6}-[0-9]{6}-[0-9]{6}$";
        public const string IBANPattern = "^BA[0-9]{18}$";
        public const string DesignationPattern = "^[A-Za-z0-9 .-]{1,500}$";
        
        static readonly Regex AccountNumberRegex = new Regex(AccountNumberPattern);
        static readonly Regex IBANRegex = new Regex(IBANPattern);
        static readonly Regex DesignationRegex = new Regex(DesignationPattern);

        public static bool IsValidAccountNumber(string accountNumber)
        {
            return AccountNumberRegex.IsMatch(accountNumber);
        }

        public static bool IsValidIBAN(string IBAN)
        {
            return IBANRegex.IsMatch(IBAN);
        }
        
        public static bool IsValidDesignation(string IBAN)
        {
            return DesignationRegex.IsMatch(IBAN);
        }
    }
}
