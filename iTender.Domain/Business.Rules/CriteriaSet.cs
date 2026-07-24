namespace iTender.Domain.Business.Rules
{
    public class CriteriaSet
    {
        private Dictionary<int, List<Criteria>> Criteria { get; set; }

        public CriteriaSet()
        {
            Criteria = new Dictionary<int, List<Criteria>>();
        }

        internal Grade EvaluateCriteria(CalculationContext calculationContext)
        {
            int highestGradeSatisfied = 0;

            foreach (KeyValuePair<int, List<Criteria>> gradeCriteria in Criteria)
            {
                bool success = true;

                foreach (Criteria criteria in gradeCriteria.Value)
                {
                    success = success && criteria.Evaluate(calculationContext);
                }

                if (success && gradeCriteria.Key > highestGradeSatisfied)
                {
                    highestGradeSatisfied = gradeCriteria.Key;
                }
            }

            return new Grade(highestGradeSatisfied, calculationContext.GradingContext.ClassOfWorkAppliedFor);
        }

        protected void RegisterCriteria(int grade, params Criteria[] criteria)
        {
            if (Criteria.ContainsKey(grade))
            {
                Criteria[grade].AddRange(new List<Criteria>(criteria));
            }
            else
            {
                Criteria.Add(grade, new List<Criteria>(criteria));
            }
        }
    }
}
