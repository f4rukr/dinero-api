using System.Collections.Generic;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.FileItem;

namespace Klika.Dinero.Model.DTO.Transaction.Response
{
    public class TransactionLineParseResponse
    {
        public TransactionFileitem TransactionFileitem { get; set; }
        public List<CsvParseError> Errors { get; }

        public TransactionLineParseResponse()
        {
            Errors = new List<CsvParseError>();
        }
        
        public TransactionLineParseResponse(CsvParseError apiError)
        {
            Errors = new List<CsvParseError> { apiError };
        }
    }
}