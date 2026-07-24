using iTender.Domain.Models;

namespace iTender.Domain.Business.Rules
{
    public class GradingDesignationSeven : IDesignationRule
    {
        public DesignationGrade GetGrade(List<ContractorModel> contractors)
        {
            //make sure that there are at least two contractors 
            if (contractors.Count >= 2)
            {
                //count to keep track for contractors with
                //grade 6
                int countSix = 0;
                //count to keep track for contractors with
                //grade 5
                int countFive = 0;
                //set found to false
                bool found = false;
                //loop through each contractor
                for (int i = 0; i < contractors.Count; i++)
                {
                    //check if the contractor has a grade that matches 4
                    var grade6 = contractors[i].Grades.Where(g => g.ApprovedGrade == "6" && g.StatusText == "Active").FirstOrDefault();
                    //if there is a grade increment the count
                    if (grade6 != null && contractors[i].StatusText == "Active")
                        countSix += 1;

                    //check if the contractor has a grade that matches 3
                    var grade5 = contractors[i].Grades.Where(g => g.ApprovedGrade == "5" && g.StatusText == "Active").FirstOrDefault();
                    //if there is a grade increment the count
                    if (grade5 != null && contractors[i].StatusText == "Active")
                        countFive += 1;

                    //if they are at least two with grade 4 or one with grade 4 and two with 
                    //grade 3                    
                    if (countSix >= 2 || (countSix >= 1 && countFive >= 2))
                    {
                        found = true;
                        break;
                    }
                }
                //return the grade
                if (found)
                    return new DesignationGrade() { Grade = "7" };
            }
            //none
            return new DesignationGrade() { Grade = "" };
        }
    }
}
