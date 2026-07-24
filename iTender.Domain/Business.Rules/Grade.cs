namespace iTender.Domain.Business.Rules
{
    public class Grade
    {
        public int Value { get; private set; }
        public string ClassOfWork { get; private set; }

        public Grade(int grade, string classOfWork)
        {
            this.ClassOfWork = classOfWork;
            this.Value = grade;
        }

        public override string ToString()
        {
            if (Value > 0)
                //return string.Format("{0}{1}", Value, ClassOfWork);
                return string.Format("{0}", Value);
            else if (Value == 0)
                return "1";
            else
                return "Not Applicable";
        }
    }
}
