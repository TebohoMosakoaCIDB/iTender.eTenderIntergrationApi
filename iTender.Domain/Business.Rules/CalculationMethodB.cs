namespace iTender.Domain.Business.Rules
{
    public class CalculationMethodB : CalculationMethod
    {
        public CalculationMethodB()
            : base(new MethodBFinancialGradingCriteriaSet(), new MethodBWorksGradingCriteria(), new MethodBSponsorshipAllowanceRuleSet(), 4)
        {
        }

        internal override string Name
        {
            get { return "MethodB"; }
        }

    }
}
