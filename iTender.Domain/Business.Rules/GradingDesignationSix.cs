using iTender.Domain.Models;

namespace iTender.Domain.Business.Rules
{
    public class GradingDesignationSix : IDesignationRule
    {
        public DesignationGrade GetGrade(List<ContractorModel> contractors)
        {
            //make sure that there are at least two contractors 
            if (contractors.Count >= 2)
            {
                //count to keep track for contractors with
                //grade 5
                int countFive = 0;
                //count to keep track for contractors with
                //grade 4
                int countFour = 0;
                //set found to false
                bool found = false;
                //loop through each contractor
                for (int i = 0; i < contractors.Count; i++)
                {
                    //check if the contractor has a grade that matches 4
                    var grade5 = contractors[i].Grades.Where(g => g.ApprovedGrade == "5" && g.StatusText == "Active").FirstOrDefault();
                    //if there is a grade increment the count
                    if (grade5 != null && contractors[i].StatusText == "Active")
                        countFive += 1;

                    //check if the contractor has a grade that matches 3
                    var grade4 = contractors[i].Grades.Where(g => g.ApprovedGrade == "4" && g.StatusText == "Active").FirstOrDefault();
                    //if there is a grade increment the count
                    if (grade4 != null && contractors[i].StatusText == "Active")
                        countFour += 1;

                    //if they are at least two with grade 4 or one with grade 4 and two with 
                    //grade 3                    
                    if (countFive >= 2 || (countFive >= 1 && countFour >= 2))
                    {
                        found = true;
                        break;
                    }
                }
                //return the grade
                if (found)
                    return new DesignationGrade() { Grade = "6" };
            }
            //none
            return new DesignationGrade() { Grade = "" };
        }
    }
}
