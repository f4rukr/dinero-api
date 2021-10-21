using Klika.Dinero.Model.Response;
using System.Collections.Generic;

namespace Klika.Dinero.Model.DTO.Bank.Response
{
    public class BankListResponse : ApiResponse
    {
        public List<BankResponseDTO> Banks { get; set; }
    }
}
