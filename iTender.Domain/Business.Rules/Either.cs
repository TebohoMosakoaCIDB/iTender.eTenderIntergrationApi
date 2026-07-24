namespace iTender.Domain.Business.Rules
{
    public class Either : Criteria
    {
        List<Criteria> _criteria;

        private Either(Criteria criteria)
        {
            _criteria = new List<Criteria>();
            _criteria.Add(criteria);
        }

        public Either OrThis(Criteria criteria)
        {
            if (this == criteria)
                throw new ArgumentException("Cannot add this criteria object to itself");

            if (_criteria.Contains(criteria))
                throw new ArgumentException("Cannot add the same criteria to a chain more than once");

            _criteria.Add(criteria);

            return this;
        }

        public static Either This(Criteria criteria)
        {
            return new Either(criteria);
        }

        internal override bool Evaluate(CalculationContext calculationContext)
        {
            bool criteriaPassed = false;

            _criteria.ForEach(c => criteriaPassed = criteriaPassed || c.Evaluate(calculationContext));

            return criteriaPassed;
        }
    }
}
