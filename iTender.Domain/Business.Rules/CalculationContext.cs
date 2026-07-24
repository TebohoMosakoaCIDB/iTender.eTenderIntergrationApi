namespace iTender.Domain.Business.Rules
{
    public class CalculationContext
    {
        public GradingContext GradingContext { get; private set; }
        public decimal AvailableCapital { get; internal set; }
        //public decimal LargestContract { get; set; }
        public CalculationMethod Method { get; private set; }

        public CalculationContext(CalculationMethod method, GradingContext gradingContext)
        {
            this.Method = method;
            this.GradingContext = gradingContext;
        }

        //public decimal BestAnnualTurnover { get; set; }

        //public int RegisteredProfessionalsCount { get; set; }
    }
}
