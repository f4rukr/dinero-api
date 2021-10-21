using Klika.Dinero.Model.DTO.Analytic.Request;
using Klika.Dinero.Model.DTO.Analytic.Response;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Interfaces
{
    public interface IAnalyticService
    {
        public Task<IncomeExpenseResponseDTO> GetIncomeExpenseAsync(AnalyticRequestDTO request);
        public Task<PieCharListResponse> GetExpenseByCategoriesAsync(AnalyticRequestDTO request);
        public Task<ExpenseDailyMonthListResponse> GetExpenseDailyMonthAsync(AnalyticRequestDTO request);
        public Task<ExpenseDailyListResponse> GetExpenseDailyAsync(AnalyticRequestDTO request);
    }
}
