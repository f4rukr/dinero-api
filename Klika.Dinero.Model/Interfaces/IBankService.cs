using Klika.Dinero.Model.DTO.Bank.Response;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Interfaces
{
    public interface IBankService
    {
        public Task<BankListResponse> GetBanksAsync();
    }
}
