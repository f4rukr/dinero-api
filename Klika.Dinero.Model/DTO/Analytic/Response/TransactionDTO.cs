using System;

namespace Klika.Dinero.Model.DTO.Analytic.Response
{
    public class TransactionDTO
    {
        public string Bank { get; set; }
        public string Designation { get; set; }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set { _amount = Decimal.Round(value, 2); }
        }
        public string Category { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}