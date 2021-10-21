using Klika.Dinero.Model.Response;

namespace Klika.Dinero.Model.DTO.Transaction.Response
{
    public class TransactionCSVExportResponse : ApiResponse
    {
        public TransactionCSVExport File { get; set; }
    }
}
