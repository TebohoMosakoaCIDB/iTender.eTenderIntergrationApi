namespace iTender.Domain.Business.Rules
{
    public class TurnoverCriteria : Criteria
    {
        private decimal Turnover { get; set; }

        public TurnoverCriteria(decimal turnover)
        {
            Turnover = turnover;
        }

        internal override bool Evaluate(CalculationContext calculationContext)
        {
            return calculationContext.GradingContext.BestAnnualTurnover >= Turnover;
        }
    }
}
