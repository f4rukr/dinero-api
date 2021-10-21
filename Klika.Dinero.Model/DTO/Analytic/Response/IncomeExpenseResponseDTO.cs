using Klika.Dinero.Model.Response;
using System;

namespace Klika.Dinero.Model.DTO.Analytic.Response
{
    public class IncomeExpenseResponseDTO : ApiResponse
    {
        private decimal _income;
        public decimal Income
        {
            get => _income;
            set { _income = Decimal.Round(value, 2); }
        }

        private decimal _expense;
        public decimal Expense
        {
            get => _expense;
            set { _expense = Decimal.Round(value, 2); }
        }
    }
}
