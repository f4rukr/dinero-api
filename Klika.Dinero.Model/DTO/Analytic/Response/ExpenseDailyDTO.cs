using System;

namespace Klika.Dinero.Model.DTO.Analytic.Response
{
    public class ExpenseDailyDTO 
    { 
        public DateTime Date { get; set; }

        private decimal _amount;
        public decimal Amount { 
            get => _amount;
            set { _amount = Decimal.Round(value, 2); } 
        }

        public ExpenseDailyDTO(DateTime date, decimal amount)
        {
            Date = date;
            Amount = amount;
        }
    }
}