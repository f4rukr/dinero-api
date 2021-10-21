using Klika.Dinero.Model.Response;

namespace Klika.Dinero.Model.DTO.Bank.Response
{
    public class BankResponseDTO : ApiResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
