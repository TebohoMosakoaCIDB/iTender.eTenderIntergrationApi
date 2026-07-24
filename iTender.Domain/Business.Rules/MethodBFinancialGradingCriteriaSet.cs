namespace iTender.Domain.Business.Rules
{
    public class MethodBFinancialGradingCriteriaSet : CriteriaSet
    {
        public MethodBFinancialGradingCriteriaSet()
        {
            RegisterCriteria(5, new AvailableCapitalCriteria(1300000));
            RegisterCriteria(6, new AvailableCapitalCriteria(2600000));
            RegisterCriteria(7, new AvailableCapitalCriteria(8000000));
            RegisterCriteria(8, new AvailableCapitalCriteria(26000000));
            RegisterCriteria(9, new AvailableCapitalCriteria(80000000));
        }
    }
}
