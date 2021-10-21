using Klika.Dinero.Model.DTO.Transaction.Response;
using Klika.Dinero.Model.Errors;
using System.Collections.Generic;

namespace Klika.Dinero.Model.Interfaces
{
    public interface IFileGeneratorService
    {
        public byte[] GenerateErrorsExcelDocument(List<ApiError> apiErrors);
        public byte[] GenerateTransactionsExcelDocument(List<TransactionCSVExportItemDTO> transactions);
    }
}
