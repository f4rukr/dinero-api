using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Klika.Dinero.Model.Entities
{
    public class Account : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Bank Bank { get; set; }
        public int BankId { get; set; }

        [MaxLength(30)]
        public string AccountNumber { get; set; }
        public string UserId { get; set; }

        private string _iban;
        [MaxLength(34)]
        public string IBAN { 
            get => _iban; 
            set { _iban = string.IsNullOrEmpty(value) ? null : value; } 
        }

        [Column(TypeName = "nvarchar(3)")]
        public CurrencyCode CurrencyCode { get; set; }
        public List<Transaction> Transactions { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Account()
        {
        }
        
        public Account(int bankId, string accountNumber, string iban, string userId)
        {
            BankId = bankId;
            UserId = userId;
            AccountNumber = accountNumber;
            IBAN = iban;
            CurrencyCode = CurrencyCode.BAM;
        }
    }
    
    public enum CurrencyCode
    {
        BAM = 1
    }
}
