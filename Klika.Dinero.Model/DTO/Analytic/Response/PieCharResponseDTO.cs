using Klika.Dinero.Model.DTO.TransactionCategory.Response;
using Klika.Dinero.Model.Response;
using System;

namespace Klika.Dinero.Model.DTO.Analytic.Response
{
    public class PieCharReponseDTO
    {
        public TransactionCategoryResponseDTO TransactionCategory { get; set; }
        public decimal ExpensePercentage { get; set; }

        private decimal _expense;
        public decimal Expense
        {
            get => _expense;
            set { _expense = Decimal.Round(value, 2); }
        }
    }
}
