using System;

namespace Klika.Dinero.Model.DTO.Transaction.Response
{
    public class TransactionCSVExportItemDTO
    {
        public DateTime DateOfTransaction { get; set; }
        public string Designation { get; set; }
        
        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set { _amount = Decimal.Round(value, 2); }
        }
        public string Bank { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
    }
}
