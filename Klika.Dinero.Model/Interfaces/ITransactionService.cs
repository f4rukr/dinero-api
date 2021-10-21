using System.Threading.Tasks;
using Klika.Dinero.Model.DTO.Transaction.Request;
using Klika.Dinero.Model.DTO.Transaction.Response;
using Klika.Dinero.Model.DTO.TransactionCategory.Response;
using Klika.Dinero.Model.Response;
using Microsoft.AspNetCore.Http;

namespace Klika.Dinero.Model.Interfaces
{
    public interface ITransactionService
    {
        public Task<ApiResponse> ValidateCsvTransactionsFileAsync(IFormFile file);   
        public Task<TransactionUploadResponse> ParseCsvTransactionsAsync(TransactionUploadRequestDTO dto);
        public Task<TransactionCategoryListResponse> GetTransactionCategoriesAsync();
        public Task<TransactionResponseDTO> CreateTransactionAsync(TransactionRequestDTO dto);
        public Task<TransactionsResponseDTO> GetTransactionsByFiltersAsync(TransactionParametersRequestDTO dto);
        public Task<TransactionCSVExportResponse> GetTransactionsForExportAsync(TransactionCSVExportRequestDTO request);
        public Task ExportTransactionsToMailAsync(TransactionCSVExportRequestDTO request);
    }
}