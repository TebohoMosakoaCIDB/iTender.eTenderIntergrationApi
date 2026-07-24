using iTender.Application.DTOs;

namespace iTender.Application.Interfaces
{
    public interface IJointVentureRepository
    {
        Task<JVGradingDesignationModel> GetRecommendedGrade(JVGradingDesignationModel model, CancellationToken ct = default);
    }
}
