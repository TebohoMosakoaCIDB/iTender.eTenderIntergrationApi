namespace iTender.Domain.Business.Rules
{
    public class NonApplicableGradingRecommendation : GradingRecommendation
    {
        internal NonApplicableGradingRecommendation(CalculationContext context, CalculationMethod method)
            : base(new Grade(-1, ""), context, method)
        {

        }

        public override string GetSummary()
        {
            return string.Format("Grade Calculation Not Applicable for {0}", Method.Name);
        }
    }
}
