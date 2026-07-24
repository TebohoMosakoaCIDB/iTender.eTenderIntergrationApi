namespace iTender.Domain.Business.Rules
{
    public interface IGradingContextFactory
    {
        Task<GradingContext> CreateContext(Guid classOfWorkId);
    }
}
