namespace iTender.Domain.Business.Rules
{
    public abstract class Capability
    {
        public Grade GradeAchieved { get; private set; }
        public Capability(Grade grade)
        {
            this.GradeAchieved = grade;
        }
    }
}
