namespace iTender.Domain.Business.Rules
{
    public class MethodAFinancialGradingCriteriaSet : CriteriaSet
    {
        public MethodAFinancialGradingCriteriaSet(DateTime ApplicationReceivedDate)
        {
            //TODO DATE NEEDS TO BE USED HERE
            //IF DATE < X then
            DateTime x = new DateTime(2013, 08, 01);

            if (ApplicationReceivedDate < x)
            {
                RegisterCriteria(1, new LargestContractCriteria(0), new TurnoverCriteria(0), new AvailableCapitalCriteria(0));
                RegisterCriteria(2, new LargestContractCriteria(150000), new TurnoverCriteria(0), new AvailableCapitalCriteria(0));
                RegisterCriteria(3, new LargestContractCriteria(500000), Either.This(new TurnoverCriteria(1000000)).OrThis(new AvailableCapitalCriteria(100000)));
                RegisterCriteria(4, new LargestContractCriteria(1000000), Either.This(new TurnoverCriteria(2000000)).OrThis(new AvailableCapitalCriteria(200000)));
                RegisterCriteria(5, new LargestContractCriteria(1600000), new TurnoverCriteria(3250000), new AvailableCapitalCriteria(650000));
                RegisterCriteria(6, new LargestContractCriteria(3250000), new TurnoverCriteria(7800000), new AvailableCapitalCriteria(1300000));
                RegisterCriteria(7, new LargestContractCriteria(10000000), new TurnoverCriteria(24000000), new AvailableCapitalCriteria(4000000));
                RegisterCriteria(8, new LargestContractCriteria(32500000), new TurnoverCriteria(90000000), new AvailableCapitalCriteria(13000000));
                RegisterCriteria(9, new LargestContractCriteria(100000000), new TurnoverCriteria(270000000), new AvailableCapitalCriteria(40000000));
            }
            else
            {
                RegisterCriteria(1, new LargestContractCriteria(0), new TurnoverCriteria(0), new AvailableCapitalCriteria(0));
                RegisterCriteria(2, new LargestContractCriteria(130000), new TurnoverCriteria(0), new AvailableCapitalCriteria(0));
                RegisterCriteria(3, new LargestContractCriteria(450000), Either.This(new TurnoverCriteria(1000000)).OrThis(new AvailableCapitalCriteria(100000)));
                RegisterCriteria(4, new LargestContractCriteria(900000), Either.This(new TurnoverCriteria(2000000)).OrThis(new AvailableCapitalCriteria(200000)));
                RegisterCriteria(5, new LargestContractCriteria(1500000), new TurnoverCriteria(3250000), new AvailableCapitalCriteria(650000));
                RegisterCriteria(6, new LargestContractCriteria(3000000), new TurnoverCriteria(6500000), new AvailableCapitalCriteria(1300000));
                RegisterCriteria(7, new LargestContractCriteria(9000000), new TurnoverCriteria(20000000), new AvailableCapitalCriteria(4000000));
                RegisterCriteria(8, new LargestContractCriteria(30000000), new TurnoverCriteria(65000000), new AvailableCapitalCriteria(13000000));
                RegisterCriteria(9, new LargestContractCriteria(90000000), new TurnoverCriteria(200000000), new AvailableCapitalCriteria(40000000));
            }
        }
    }
}
