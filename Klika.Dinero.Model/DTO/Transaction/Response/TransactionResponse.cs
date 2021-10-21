using System.Collections.Generic;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Response;

namespace Klika.Dinero.Model.DTO.Transaction.Response
{
    public class TransactionResponseDTO : ApiResponse
    {
        public Entities.Transaction Transaction { get; set; }

        public TransactionResponseDTO(Entities.Transaction transaction)
        {
            Transaction = transaction;
        }
        
        public TransactionResponseDTO(ApiError error)
        {
            Errors = new List<ApiError> { error };
        }
    }
}