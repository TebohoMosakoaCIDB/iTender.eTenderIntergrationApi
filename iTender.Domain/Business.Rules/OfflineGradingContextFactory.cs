namespace iTender.Domain.Business.Rules
{
    public class OfflineGradingContextFactory : IGradingContextFactory
    {
        public decimal BestAnnualTurnover { get; set; }
        public decimal LargestContractValue { get; set; }
        public decimal LargestCowContractValue { get; set; }
        public decimal AvailableCapital { get; set; }
        public int QualifiedProfessionals { get; set; }
        public bool ElectricalLicense { get; set; }
        public string ClassOfWork { get; set; }
        public int GradeAppliedFor { get; set; }
        public DateTime ApplicationReceivedDate { get; set; }

        public OfflineGradingContextFactory(
            decimal bestAnnualTurnover,
            decimal largestContractValue,
            decimal contractValue,
            decimal availableCapital,
            int qualifiedProfessionals,
            bool electricalLicense,
            string classOfWork,
            DateTime applicationReceivedDate)
        {
            BestAnnualTurnover = bestAnnualTurnover;
            LargestContractValue = largestContractValue;
            LargestCowContractValue = contractValue;
            AvailableCapital = availableCapital;
            QualifiedProfessionals = qualifiedProfessionals;
            ElectricalLicense = electricalLicense;
            ClassOfWork = classOfWork;
            GradeAppliedFor = 1;
            ApplicationReceivedDate = applicationReceivedDate;
        }

        public async Task<GradingContext> CreateContext(Guid classOfWorkId)
        {
            return new GradingContext
            {
                BestAnnualTurnover = BestAnnualTurnover,
                ClassOfWorkAppliedFor = ClassOfWork,
                GradeAppliedFor = GradeAppliedFor,
                LargestContract = LargestContractValue,
                LargestContractForCow = LargestCowContractValue,
                NetAssetValue = AvailableCapital,
                RegisteredProfessionalsCount = QualifiedProfessionals,
                ApplicationReceivedDate = ApplicationReceivedDate,
                Sponsorships = new List<SponsorshipSummary>()
            };
        }
    }
}
