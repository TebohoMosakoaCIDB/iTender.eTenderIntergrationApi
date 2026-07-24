namespace iTender.Domain.Business.Rules
{
    public abstract class CalculationMethod
    {
        internal CriteriaSet FinancialGradingCriteriaSet { get; private set; }
        internal CriteriaSet WorksGradingCriteriaSet { get; private set; }
        internal SponsorshipAllowanceRuleSet SponsorhsipAllowanceRuleSet { get; private set; }
        internal int LowestApplicableGrade { get; private set; }

        internal abstract string Name { get; }

        /// <summary>
        /// Default constructor will use 1 as the default value for "lowestGradeApplicable"
        /// </summary>
        /// <param name="financialGradingCriteria"></param>
        /// <param name="worksGradingCritera"></param>
        /// <param name="sponsorshipAllowanceRuleSet"></param>
        internal CalculationMethod(CriteriaSet financialGradingCriteria, CriteriaSet worksGradingCritera, SponsorshipAllowanceRuleSet sponsorshipAllowanceRuleSet)
            : this(financialGradingCriteria, worksGradingCritera, sponsorshipAllowanceRuleSet, 1)
        {
        }

        internal CalculationMethod(CriteriaSet financialGradingCriteria, CriteriaSet worksGradingCritera, SponsorshipAllowanceRuleSet sponsorshipAllowanceRuleSet, int lowestApplicableGrade)
        {
            FinancialGradingCriteriaSet = financialGradingCriteria;
            WorksGradingCriteriaSet = worksGradingCritera;
            SponsorhsipAllowanceRuleSet = sponsorshipAllowanceRuleSet;
            LowestApplicableGrade = lowestApplicableGrade;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
