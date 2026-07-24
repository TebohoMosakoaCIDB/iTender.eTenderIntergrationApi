using iTender.Domain.Models;

namespace iTender.Domain.Business.Rules
{
    public class GradingDesignationFive : IDesignationRule
    {
        public DesignationGrade GetGrade(List<ContractorModel> contractors)
        {
            //make sure that there are at least two contractors 
            if (contractors.Count >= 2)
            {
                //count to keep track for contractors with
                //grade 4
                int countFour = 0;
                //count to keep track for contractors with
                //grade 3
                int countThree = 0;
                //set found to false
                bool found = false;
                //loop through each contractor
                for (int i = 0; i < contractors.Count; i++)
                {
                    //check if the contractor has a grade that matches 4
                    var grade4 = contractors[i].Grades.Where(g => g.ApprovedGrade == "4" && g.StatusText == "Active").FirstOrDefault();
                    //if there is a grade increment the count
                    if (grade4 != null && contractors[i].StatusText == "Active")
                        countFour += 1;

                    //check if the contractor has a grade that matches 3
                    var grade3 = contractors[i].Grades.Where(g => g.ApprovedGrade == "3" && g.StatusText == "Active").FirstOrDefault();
                    //if there is a grade increment the count
                    if (grade3 != null && contractors[i].StatusText == "Active")
                        countThree += 1;

                    //if they are at least two with grade 4 or one with grade 4 and two with 
                    //grade 3                    
                    if (countFour >= 2 || (countFour >= 1 && countThree >= 2))
                    {
                        found = true;
                        break;
                    }
                }
                //return the grade
                if (found)
                    return new DesignationGrade() { Grade = "5" };
            }
            //none
            return new DesignationGrade() { Grade = "" };
        }
    }
}
