using System.Collections.Generic;
using System.Threading.Tasks;
using Klika.Dinero.Model.DTO.Transaction.Response;
using Microsoft.AspNetCore.Http;

namespace Klika.Dinero.Model.Interfaces
{    
    public interface ICsvTransactionParser
    {
        public Task<string> ReadHeadersAsync(IFormFile file);
        public Task<List<string>> ReadLinesAsync(IFormFile file);
        public Task<TransactionsParseResponse> ParseAndValidateCSVAsync(IFormFile file);
        public TransactionLineParseResponse FromCsv(string csvLine, long index);
    }
}