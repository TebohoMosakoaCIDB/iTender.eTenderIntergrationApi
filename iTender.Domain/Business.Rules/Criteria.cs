namespace iTender.Domain.Business.Rules
{
    public abstract class Criteria
    {
        internal abstract bool Evaluate(CalculationContext calculationContext);
    }
}
