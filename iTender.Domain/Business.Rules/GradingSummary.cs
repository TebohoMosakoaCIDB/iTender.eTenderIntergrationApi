using System.Text;

namespace iTender.Domain.Business.Rules
{
    public class GradingSummary
    {
        private GradingRecommendation _gradingRecommendation;
        public GradingSummary(GradingRecommendation gradingRecommendation)
        {
            _gradingRecommendation = gradingRecommendation;
        }
        public override string ToString()
        {
            string summary = BuildSummaryMessage();

            return summary;
        }

        private string BuildSummaryMessage()
        {
            StringBuilder summary = new StringBuilder();

            summary.AppendFormat("Calculation Method Used: {0}\r\n\r\n", _gradingRecommendation.Method.Name);
            summary.AppendFormat("Class of Work: {0}\r\n", _gradingRecommendation.CalculationContext.GradingContext.ClassOfWorkAppliedFor);
            summary.AppendFormat("Maximum Contract Completed: {0:c}\r\n", _gradingRecommendation.CalculationContext.GradingContext.LargestContract);
            summary.AppendFormat("Maximum Contract For Class of Work: {0:c}\r\n", _gradingRecommendation.CalculationContext.GradingContext.LargestContractForCow);
            summary.AppendFormat("Available Capital: {0:c}\r\n", _gradingRecommendation.CalculationContext.AvailableCapital);
            summary.AppendFormat("Best Turnover For Last 2 Years: {0:c}\r\n", _gradingRecommendation.CalculationContext.GradingContext.BestAnnualTurnover);
            summary.AppendFormat("Number of Full-Time Equivalent Registered Professionals: {0}\r\n", _gradingRecommendation.CalculationContext.GradingContext.RegisteredProfessionalsCount);
            summary.AppendLine();
            summary.AppendFormat("Grade Recommended: {0}", _gradingRecommendation.HighestGradeAchieved.ToString());

            return summary.ToString();
        }
    }
}
