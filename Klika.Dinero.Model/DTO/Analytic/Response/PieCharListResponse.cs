using Klika.Dinero.Model.Response;
using System.Collections.Generic;

namespace Klika.Dinero.Model.DTO.Analytic.Response
{
    public class PieCharListResponse : ApiResponse
    {
        public List<PieCharReponseDTO> PieChars { get; set; }
    }
}
