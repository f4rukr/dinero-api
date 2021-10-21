using System;
using System.Collections.Generic;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Response;

namespace Klika.Dinero.Model.DTO.Transaction.Response
{
    public class TransactionUploadResponse : ApiResponse
    {
        public DateTime MostRecentTransaction { get; set; }

        public TransactionUploadResponse(DateTime mostRecentTransaction)
        {
            MostRecentTransaction = mostRecentTransaction;
        }
        
        public TransactionUploadResponse(List<ApiError> errors)
        {
            Errors = errors;
        }
        
        public TransactionUploadResponse(ApiError error)
        {
            Errors = new List<ApiError>() { error };
        }
    }
}