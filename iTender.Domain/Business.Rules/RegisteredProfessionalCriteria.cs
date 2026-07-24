namespace iTender.Domain.Business.Rules
{
    public class RegisteredProfessionalCriteria : Criteria
    {
        Dictionary<string, int> professionalsPerClassOfWork = new Dictionary<string, int>();

        public RegisteredProfessionalCriteria(int gb = 0, int ce = 0, int ebep = 0, int me = 0, int specialist = 0)
        {
            professionalsPerClassOfWork["GB"] = gb;
            professionalsPerClassOfWork["CE"] = ce;
            professionalsPerClassOfWork["EB"] = ebep;
            professionalsPerClassOfWork["EP"] = ebep;
            professionalsPerClassOfWork["ME"] = me;
            professionalsPerClassOfWork["SB"] = specialist;
            professionalsPerClassOfWork["SC"] = specialist;
            professionalsPerClassOfWork["SD"] = specialist;
            professionalsPerClassOfWork["SE"] = specialist;
            professionalsPerClassOfWork["SF"] = specialist;
            professionalsPerClassOfWork["SG"] = specialist;
            professionalsPerClassOfWork["SH"] = specialist;
            professionalsPerClassOfWork["SI"] = specialist;
            professionalsPerClassOfWork["SJ"] = specialist;
            professionalsPerClassOfWork["SK"] = specialist;
            professionalsPerClassOfWork["SL"] = specialist;
            professionalsPerClassOfWork["SM"] = specialist;
            professionalsPerClassOfWork["SN"] = specialist;
            professionalsPerClassOfWork["SO"] = specialist;
            professionalsPerClassOfWork["SQ"] = specialist;
        }
        internal override bool Evaluate(CalculationContext calculationContext)
        {
            DateTime x = new DateTime(2013, 08, 01);

            if (calculationContext.GradingContext.ApplicationReceivedDate <= x)
            {
                if (professionalsPerClassOfWork.ContainsKey(calculationContext.GradingContext.ClassOfWorkAppliedFor))
                {
                    return calculationContext.GradingContext.RegisteredProfessionalsCount >= professionalsPerClassOfWork[calculationContext.GradingContext.ClassOfWorkAppliedFor];
                }
            }

            return true;
        }
    }
}
