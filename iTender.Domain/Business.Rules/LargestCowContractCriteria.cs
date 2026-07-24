namespace iTender.Domain.Business.Rules
{
    public class LargestCowContractCriteria : Criteria
    {
        private decimal LargestCowContract { get; set; }

        public LargestCowContractCriteria(decimal largestCowContract)
        {
            this.LargestCowContract = largestCowContract;
        }

        internal override bool Evaluate(CalculationContext calculationContext)
        {
            return calculationContext.GradingContext.LargestContractForCow >= LargestCowContract;
        }
    }
}
