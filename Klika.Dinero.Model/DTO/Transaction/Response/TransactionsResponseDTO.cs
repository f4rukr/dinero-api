using Klika.Dinero.Model.DTO.Analytic.Response;
using Klika.Dinero.Model.Response;
using System.Collections.Generic;
using Klika.Dinero.Model.Helpers.Pagination;

namespace Klika.Dinero.Model.DTO.Transaction.Response
{
    public class TransactionsResponseDTO : ApiResponse
    {
        public LoadMoreMeta Metadata { get; set; }
        public List<TransactionDTO> Transactions { get; set; }
    }
}
