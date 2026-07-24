using iTender.Domain.Enums;

namespace iTender.Domain.Business.Rules
{
    public class MethodBSponsorshipAllowanceRuleSet : SponsorshipAllowanceRuleSet
    {
        public MethodBSponsorshipAllowanceRuleSet()
        {
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 2, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 3, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 4, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 5, 1300000);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 6, 2600000);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 7, 8000000);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 8, 26000000);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 9, 80000000);

            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 2, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 3, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 4, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 5, 975000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 6, 1950000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 7, 6000000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 8, 19500000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 9, 60000000);

            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 2, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 3, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 4, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 5, 650000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 6, 1300000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 7, 4000000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 8, 13000000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 9, 40000000);
        }
    }
}
