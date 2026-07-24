using iTender.Domain.Enums;

namespace iTender.Domain.Business.Rules
{
    public class SponsorshipAllowanceRuleSet
    {
        public Dictionary<SponsorshipType, Dictionary<int, Decimal>> TypeGradeContributionLimits { get; private set; }
        public SponsorshipAllowanceRuleSet()
        {
            TypeGradeContributionLimits = new Dictionary<SponsorshipType, Dictionary<int, decimal>>();
        }

        internal decimal GetSponsorshipAllowedContribution(SponsorshipSummary sponsorship, CalculationContext calculationContext)
        {
            if (sponsorship.Type != SponsorshipType.FinancialSponsor)
            {
                if (TypeGradeContributionLimits.ContainsKey(sponsorship.Type) && TypeGradeContributionLimits[sponsorship.Type].ContainsKey(calculationContext.GradingContext.GradeAppliedFor))
                {
                    Decimal sponsorshipAllowance = TypeGradeContributionLimits[sponsorship.Type][calculationContext.GradingContext.GradeAppliedFor];

                    if (sponsorship.Amount > sponsorshipAllowance)
                    {
                        return sponsorshipAllowance;
                    }
                    else
                    {
                        return sponsorship.Amount;
                    }
                }
            }
            else
            {
                return sponsorship.Amount;
            }

            return 0m;
        }

        protected void RegisterSponsorshipContributionLimit(SponsorshipType sponsorshipType, int grade, decimal sponsorshipContributionLimit)
        {
            if (!this.TypeGradeContributionLimits.ContainsKey(sponsorshipType))
            {
                this.TypeGradeContributionLimits.Add(sponsorshipType, new Dictionary<int, decimal>());
            }

            Dictionary<int, decimal> typeDictionary = this.TypeGradeContributionLimits[sponsorshipType];

            if (!typeDictionary.ContainsKey(grade))
            {
                typeDictionary.Add(grade, sponsorshipContributionLimit);
            }
        }
    }
}
