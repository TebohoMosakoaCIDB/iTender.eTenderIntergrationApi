namespace iTender.Domain.Business.Rules
{
    public class GradingCalculator
    {
        private readonly IGradingContextFactory _contextFactory;

        public GradingCalculator(IGradingContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GradeCalculationResult> CalculateRecommendedGrades(Guid classOfWorkId)
        {
            var context = await _contextFactory.CreateContext(classOfWorkId); 

            var calculator = new Calculator(context);

            var methodA = new CalculationMethodA(context.ApplicationReceivedDate);
            var methodB = new CalculationMethodB();

            var recA = calculator.Calculate(methodA);

            GradingRecommendation recB;

            var cutoffDate = new DateTime(2013, 08, 01);

            if (context.ApplicationReceivedDate < cutoffDate)
            {
                recB = calculator.Calculate(methodB);
            }
            else
            {
                recB = new GradingRecommendation(
                    new Grade(-1, ""),
                    new CalculationContext(methodB, context),
                    methodB);
            }

            return new GradeCalculationResult(recA, recB);
        }
    }
}
