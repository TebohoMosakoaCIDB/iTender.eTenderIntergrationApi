namespace iTender.Domain.Business.Rules
{
    public class GradingContext
    {
        //private nv_classofwork _classOfWork;
        public IEnumerable<SponsorshipSummary> Sponsorships { get; set; }

        public decimal NetAssetValue { get; set; }

        public int GradeAppliedFor { get; set; }

        public string ClassOfWorkAppliedFor { get; set; }

        public int RegisteredProfessionalsCount { get; set; }

        public decimal LargestContract { get; set; }

        public decimal LargestContractForCow { get; set; }

        public decimal BestAnnualTurnover { get; set; }

        public DateTime ApplicationReceivedDate { get; set; }

        internal GradingContext()
        {
        }
    }
}
