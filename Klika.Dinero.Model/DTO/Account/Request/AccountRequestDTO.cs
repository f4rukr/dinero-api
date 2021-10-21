using Klika.Dinero.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Klika.Dinero.Model.Constants.RegexConstants;

namespace Klika.Dinero.Model.DTO.Account.Request
{
    public class AccountRequestDTO
    {
        [JsonIgnore]
        public string UserId { get; set; }

        [Required]
        [RegularExpression(RegexValidators.AccountNumberPattern, ErrorMessage = RegexMessages.InvalidAccountNumber)]
        public string AccountNumber { get; set; }

        [RegularExpression(RegexValidators.IBANPattern, ErrorMessage = RegexMessages.InvalidIBAN)]
        public string IBAN { get; set; }
        
        [Required]
        public int BankId { get; set; }

        [Required]
        public CurrencyCode CurrencyCode { get; set; } = CurrencyCode.BAM;
    }
}
