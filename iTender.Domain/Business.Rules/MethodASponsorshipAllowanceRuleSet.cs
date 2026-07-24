using iTender.Domain.Enums;

namespace iTender.Domain.Business.Rules
{
    public class MethodASponsorshipAllowanceRuleSet : SponsorshipAllowanceRuleSet
    {
        public MethodASponsorshipAllowanceRuleSet()
        {
            //TODO ADD DATE IF HERE || ASK EBRAHIM
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 2, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 3, 100000.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 4, 200000.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 5, 650000.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 6, 1300000.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 7, 4000000.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 8, 13000000.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.CidbContractorOrFiftyPercentPlusShareholder, 9, 40000000.0m);


            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 2, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 3, 75000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 4, 150000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 5, 487500);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 6, 975000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 7, 3000000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 8, 9750000);
            RegisterSponsorshipContributionLimit(SponsorshipType.TwentyFiveToFiftyPercentShareholder, 9, 30000000);


            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 2, 0.0m);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 3, 50000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 4, 100000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 5, 325000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 6, 650000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 7, 2000000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 8, 6500000);
            RegisterSponsorshipContributionLimit(SponsorshipType.SubTwentyFivePercentShareholder, 9, 20000000);
        }
    }
}
