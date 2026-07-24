namespace iTender.Domain.Business.Rules
{
    public class GradingRecommendation
    {
        internal CalculationContext CalculationContext { get; set; }
        public CalculationMethod Method { get; private set; }
        public Grade HighestGradeAchieved { get; private set; }
        internal GradingSummary Summary { get; private set; }

        internal GradingRecommendation(Grade highestGradeAchieved, CalculationContext context, CalculationMethod method)
        {
            this.CalculationContext = context;
            this.HighestGradeAchieved = highestGradeAchieved;
            this.Method = method;
            Summary = new GradingSummary(this);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Method, HighestGradeAchieved);
        }

        /// <summary>
        /// Returns a textual summary containing all the parameters used during the grading calculation
        /// </summary>
        /// <returns></returns>
        public virtual string GetSummary()
        {
            return Summary.ToString();
        }
    }
}
