namespace iTender.Domain.Business.Rules
{
    public class MethodAWorksGradingCriteria : CriteriaSet
    {
        public MethodAWorksGradingCriteria(DateTime ApplicationReceivedDate)
        {
            DateTime x = new DateTime(2013, 08, 01);

            if (ApplicationReceivedDate < x)
            {
                RegisterCriteria(1, new LargestCowContractCriteria(0));
                RegisterCriteria(2, new LargestCowContractCriteria(150000));
                RegisterCriteria(3, new LargestCowContractCriteria(500000));
                RegisterCriteria(4, new LargestCowContractCriteria(1000000));
                RegisterCriteria(5, new LargestCowContractCriteria(1600000));
                RegisterCriteria(6, new LargestCowContractCriteria(3250000), new RegisteredProfessionalCriteria(ebep: 1, me: 1, specialist: 1));
                RegisterCriteria(7, new LargestCowContractCriteria(10000000), new RegisteredProfessionalCriteria(gb: 1, ce: 1, ebep: 2, me: 2, specialist: 2));
                RegisterCriteria(8, new LargestCowContractCriteria(32500000), new RegisteredProfessionalCriteria(gb: 2, ce: 2, ebep: 3, me: 3, specialist: 3));
                RegisterCriteria(9, new LargestCowContractCriteria(100000000), new RegisteredProfessionalCriteria(gb: 3, ce: 3, ebep: 4, me: 4, specialist: 4));
            }
            else
            {
                RegisterCriteria(1, new LargestCowContractCriteria(0));
                RegisterCriteria(2, new LargestCowContractCriteria(130000));
                RegisterCriteria(3, new LargestCowContractCriteria(450000));
                RegisterCriteria(4, new LargestCowContractCriteria(900000));
                RegisterCriteria(5, new LargestCowContractCriteria(1500000));
                RegisterCriteria(6, new LargestCowContractCriteria(3000000), new RegisteredProfessionalCriteria(ebep: 1, me: 1, specialist: 1));
                RegisterCriteria(7, new LargestCowContractCriteria(9000000), new RegisteredProfessionalCriteria(gb: 1, ce: 1, ebep: 2, me: 2, specialist: 2));
                RegisterCriteria(8, new LargestCowContractCriteria(30000000), new RegisteredProfessionalCriteria(gb: 2, ce: 2, ebep: 3, me: 3, specialist: 3));
                RegisterCriteria(9, new LargestCowContractCriteria(90000000), new RegisteredProfessionalCriteria(gb: 3, ce: 3, ebep: 4, me: 4, specialist: 4));
            }
        }
    }
}
