namespace iTender.Domain.Business.Rules
{
    public class GradeCalculationResult
    {
        public GradingRecommendation MethodARecommendedGrade { get; private set; }
        public GradingRecommendation MethodBRecommendedGrade { get; private set; }

        internal GradeCalculationResult(GradingRecommendation methodA, GradingRecommendation methodB)
        {
            MethodARecommendedGrade = methodA;
            MethodBRecommendedGrade = methodB;
        }
    }
}
