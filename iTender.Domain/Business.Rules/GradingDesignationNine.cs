using iTender.Domain.Models;

namespace iTender.Domain.Business.Rules
{
    public class GradingDesignationNine : IDesignationRule
    {
        public DesignationGrade GetGrade(List<ContractorModel> contractors)
        {
            //make sure that there are three contractors 
            if (contractors.Count >= 3)
            {
                //count to keep track of grades found 
                //for each contractor
                int count = 0;
                //set found to false
                bool found = false;
                //loop through each contractor
                for (int i = 0; i < contractors.Count; i++)
                {
                    //check if the contractor has a grade that matches 9
                    var grade = contractors[i].Grades.Where(g => g.ApprovedGrade == "8" && g.StatusText == "Active").FirstOrDefault();
                    //if there is a grade increment the count
                    if (grade != null && contractors[i].StatusText == "Active")
                        count += 1;
                    //if they are at least three then you award
                    //the grade
                    if (count >= 3)
                    {
                        found = true;
                        break;
                    }
                }
                //return the grade
                if (found)
                    return new DesignationGrade() { Grade = "9" };
            }
            //none
            return new DesignationGrade() { Grade = "" };
        }
    }
}
