namespace iTender.Domain.Business.Rules
{
    public class MethodBWorksGradingCriteria : CriteriaSet
    {
        public MethodBWorksGradingCriteria()
        {
            RegisterCriteria(5, new RegisteredProfessionalCriteria(gb: 1, ce: 1, ebep: 1, me: 1, specialist: 1));
            RegisterCriteria(6, new RegisteredProfessionalCriteria(gb: 2, ce: 2, ebep: 2, me: 2, specialist: 2));
            RegisterCriteria(7, new RegisteredProfessionalCriteria(gb: 4, ce: 4, ebep: 4, me: 4, specialist: 4));
            RegisterCriteria(8, new RegisteredProfessionalCriteria(gb: 6, ce: 6, ebep: 6, me: 6, specialist: 6));
            RegisterCriteria(9, new RegisteredProfessionalCriteria(gb: 8, ce: 8, ebep: 8, me: 8, specialist: 8));
        }
    }
}
