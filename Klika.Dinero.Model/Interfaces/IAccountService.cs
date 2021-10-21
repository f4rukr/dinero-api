using Klika.Dinero.Model.DTO.Account.Request;
using Klika.Dinero.Model.DTO.Account.Response;
using Klika.Dinero.Model.Response;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Interfaces
{
    public interface IAccountService
    {
        public Task<AccountResponseDTO> GetAccountAsync(string accountNumber, string userId);
        public Task<AccountListResponse> GetAccountsAsync(string userId);
        public Task<AccountResponseDTO> CreateAccountAsync(AccountRequestDTO request);
        public Task<ApiResponse> DeleteAccountAsync(string accountNumber, string userId);
    }
}
