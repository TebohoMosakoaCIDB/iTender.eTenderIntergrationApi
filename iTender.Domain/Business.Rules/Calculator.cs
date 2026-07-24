namespace iTender.Domain.Business.Rules
{
    public class Calculator
    {
        private GradingContext _gradingContext;
        public Calculator(GradingContext gradingContext)
        {
            _gradingContext = gradingContext;
        }

        public GradingRecommendation Calculate(CalculationMethod method)
        {
            CalculationContext calculationContext = new CalculationContext(method, _gradingContext);

            if (_gradingContext.GradeAppliedFor < method.LowestApplicableGrade)
                return new NonApplicableGradingRecommendation(calculationContext, method);

            //Calculate Available Capital
            CalculateAvailableCapital(calculationContext);

            //Determine Financial Capability
            FinancialCapability financialCapability = CalculateFinancialCapability(calculationContext);

            //Determine Works Capability
            WorksCapability worksCapability = CalculateWorksCapability(calculationContext);

            //Determine Lowest Grade Capability Achieved
            Grade lowestGradeCapibility = financialCapability.GradeAchieved;

            if (worksCapability.GradeAchieved.Value < lowestGradeCapibility.Value)
            {
                lowestGradeCapibility = worksCapability.GradeAchieved;
            }

            return new GradingRecommendation(lowestGradeCapibility, calculationContext, calculationContext.Method);
        }

        private WorksCapability CalculateWorksCapability(CalculationContext calculationContext)
        {
            CriteriaSet criteriaSet = calculationContext.Method.WorksGradingCriteriaSet;

            Grade gradeAchieved = criteriaSet.EvaluateCriteria(calculationContext);

            return new WorksCapability(gradeAchieved);
        }

        private FinancialCapability CalculateFinancialCapability(CalculationContext calculationContext)
        {
            CriteriaSet criteriaSet = calculationContext.Method.FinancialGradingCriteriaSet;

            Grade gradeAchieved = criteriaSet.EvaluateCriteria(calculationContext);

            return new FinancialCapability(gradeAchieved);
        }

        private void CalculateAvailableCapital(CalculationContext calculationContext)
        {
            Decimal netAssetValue = calculationContext.GradingContext.NetAssetValue;

            Decimal sponsorshipValue = 0.0m;

            IEnumerable<SponsorshipSummary> sponsorships = calculationContext.GradingContext.Sponsorships;

            foreach (SponsorshipSummary sponsorship in sponsorships)
            {
                SponsorshipAllowanceRuleSet sponsorshipRuleSet = calculationContext.Method.SponsorhsipAllowanceRuleSet;
                sponsorshipValue += sponsorshipRuleSet.GetSponsorshipAllowedContribution(sponsorship, calculationContext);
            }

            calculationContext.AvailableCapital = sponsorshipValue + netAssetValue;
        }
    }
}
