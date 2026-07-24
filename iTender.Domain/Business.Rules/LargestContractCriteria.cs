namespace iTender.Domain.Business.Rules
{
    public class LargestContractCriteria : Criteria
    {
        private decimal LargestContract { get; set; }

        public LargestContractCriteria(decimal largestContract)
        {
            this.LargestContract = largestContract;
        }

        internal override bool Evaluate(CalculationContext calculationContext)
        {
            return calculationContext.GradingContext.LargestContract >= LargestContract;
        }
    }
}
