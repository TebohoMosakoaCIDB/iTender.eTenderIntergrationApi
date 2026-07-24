namespace iTender.Domain.Models
{
    public class ContractorDevelopmentProgrammeModel
    {
        public Guid Id { get; set; }
        public Guid EmployerId { get; set; }
        public int PrimaryFocus { get; set; }
        public string PrimaryFocusText { get; set; }
        public string TotalBudgetAmount { get; set; }
        public string TotalBudgetAmountRands { get; set; }
        public string TotalBudgetAmountCents { get; set; }
        public string CDPName { get; set; }
        public string CDPNumber { get; set; }
        public string EmployerCRS { get; set; }
        public string EmployerName { get; set; }
        public string Province { get; set; }
        public List<ContractorModel> ContractorSubmissions { get; set; } = new List<ContractorModel>();
        public int SubmissionCount { get; set; }
    }
}
