using System.Collections.Generic;
using Klika.Dinero.Model.Response;

namespace Klika.Dinero.Model.DTO.Analytic.Response
{
    public class ExpenseDailyListResponse : ApiResponse
    {
        public List<TransactionDTO> Expenses { get; set; }
    }
}