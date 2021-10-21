using Klika.Dinero.Model.Response;

namespace Klika.Dinero.Model.DTO.TransactionCategory.Response
{
    public class TransactionCategoryResponseDTO : ApiResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
}
