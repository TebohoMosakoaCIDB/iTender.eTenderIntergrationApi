using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IGradingRepository
    {
        Task<RecommendedGradeModel> GetByGradeAsync(string grade);
    }
}
