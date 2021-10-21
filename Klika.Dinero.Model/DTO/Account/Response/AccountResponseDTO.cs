using Klika.Dinero.Model.DTO.Bank.Response;
using Klika.Dinero.Model.Entities;
using Klika.Dinero.Model.Response;
using System;

namespace Klika.Dinero.Model.DTO.Account.Response
{
    public class AccountResponseDTO : ApiResponse
    {
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public BankResponseDTO Bank { get; set; }
        public DateTime? DateOfLastTransaction { get; set; }
        public CurrencyCode CurrencyCode { get; set; }

        private decimal _balance;
        public decimal Balance { 
            get => _balance;
            set { _balance = Decimal.Round(value, 2); }
        }
    }
}
