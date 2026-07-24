namespace iTender.Domain.Business.Rules
{
    public class CalculationMethodA : CalculationMethod
    {
        public CalculationMethodA(DateTime ApplicationReceivedDate)
            : base(new MethodAFinancialGradingCriteriaSet(ApplicationReceivedDate), new MethodAWorksGradingCriteria(ApplicationReceivedDate), new MethodASponsorshipAllowanceRuleSet())
        {
        }

        internal override string Name
        {
            get { return "MethodA"; }
        }
    }
}
