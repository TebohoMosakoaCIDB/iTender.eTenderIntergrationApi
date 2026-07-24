using iTender.Domain.Models;

namespace iTender.Domain.Business.Rules
{
    public static class ProcessGradingDesignation
    {
        static List<IDesignationRule> m_Rules;

        public static string CalculateGrade(List<ContractorModel> contractors)
        {
            string grade = "";
            //register the designation grade rules
            RegisterDesignationRules();
            //process the rule to get the grade
            foreach (IDesignationRule rule in m_Rules)
            {
                grade = rule.GetGrade(contractors).Grade;

                if (!string.IsNullOrEmpty(grade))
                    break;
            }
            return grade;
        }

        private static void RegisterDesignationRules()
        {
            //add the rules 
            m_Rules = new List<IDesignationRule>();
            m_Rules.Add(new GradingDesignationNine());
            m_Rules.Add(new GradingDesignationEight());
            m_Rules.Add(new GradingDesignationSeven());
            m_Rules.Add(new GradingDesignationSix());
            m_Rules.Add(new GradingDesignationFive());
            m_Rules.Add(new GradingDesignationFour());
            m_Rules.Add(new GradingDesignationThree());
        }
    }
}
