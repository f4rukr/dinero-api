using Klika.Dinero.Model.Response;
using System.Collections.Generic;

namespace Klika.Dinero.Model.DTO.TransactionCategory.Response
{
    public class TransactionCategoryListResponse : ApiResponse
    {
        public List<TransactionCategoryResponseDTO> TransactionCategories { get; set; }
    }
}
