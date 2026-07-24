namespace iTender.Domain.Business.Rules
{
    public class AvailableCapitalCriteria : Criteria
    {
        private decimal AvailableCapital { get; set; }

        public AvailableCapitalCriteria(decimal availableCapital)
        {
            AvailableCapital = availableCapital;
        }

        internal override bool Evaluate(CalculationContext calculationContext)
        {
            return calculationContext.AvailableCapital >= AvailableCapital;
        }
    }
}
