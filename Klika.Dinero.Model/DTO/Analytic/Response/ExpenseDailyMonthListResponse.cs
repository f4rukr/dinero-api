using System.Collections.Generic;
using Klika.Dinero.Model.Response;

namespace Klika.Dinero.Model.DTO.Analytic.Response
{
    public class ExpenseDailyMonthListResponse : ApiResponse
    {
        public List<ExpenseDailyDTO> Expenses { get; set; }
    }
}