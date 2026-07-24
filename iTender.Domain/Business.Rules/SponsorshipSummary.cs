using iTender.Domain.Enums;

namespace iTender.Domain.Business.Rules
{
    public class SponsorshipSummary
    {
        public SponsorshipType Type { get; private set; }
        public decimal Amount { get; private set; }

        public string SponsorName { get; set; }

        public SponsorshipType SponsorType { get; set; }

        public decimal? SponsorshipValue { get; set; }

        public string RelationshipWithSponsor { get; set; }

        private SponsorshipSummary(SponsorshipType sponsorshipType, decimal sponsorshipAmount)
        {
            this.Type = sponsorshipType;
            this.Amount = sponsorshipAmount;
        }

        /// <summary>
        /// Factory method to create sponsorship summaries
        /// </summary>
        /// <param name="sponsorshipBO"></param>
        /// <param name="sponsorship"></param>
        /// <returns></returns>
        public static SponsorshipSummary Create(string sponsorshipTypeName, decimal amount)
        {
            var type = ResolveSponsorshipTypeName(sponsorshipTypeName);

            return new SponsorshipSummary(type, amount);
        }

        /// <summary>
        /// Converts friendly sponsorship type name to SponsorshipType enum
        /// </summary>
        /// <param name="sponsorShipTypeName"></param>
        /// <returns></returns>
        private static SponsorshipType ResolveSponsorshipTypeName(string sponsorShipTypeName)
        {
            switch (sponsorShipTypeName)
            {
                case "CIDB Registered Contractor":
                    return SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder;
                case "Company with more than 50% shareholding in applicant":
                    return SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder;
                case "Company with 25% or more shareholding in applicant":
                    return SponsorshipType.TwentyFiveToFiftyPercentShareholder;
                case "Company with less than 25% shareholding in applicant":
                    return SponsorshipType.SubTwentyFivePercentShareholder;
                case "Registered Financial Services Provider or development institution":
                    return SponsorshipType.FinancialSponsor;
                default:
                    throw new ArgumentException("\"" + sponsorShipTypeName + "\" is not a known sponsorship type", "sponsorShipTypeName");
            }
        }
    }
}
