using Klika.Dinero.Model.Response;
using System.Collections.Generic;

namespace Klika.Dinero.Model.DTO.Account.Response
{
    public class AccountListResponse : ApiResponse
    {
        public List<AccountResponseDTO> Accounts { get; set; }
    }
}
