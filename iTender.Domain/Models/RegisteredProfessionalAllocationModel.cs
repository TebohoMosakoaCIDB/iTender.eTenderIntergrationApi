namespace iTender.Domain.Models
{
    public class RegisteredProfessionalAllocationModel
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ContractorId { get; set; }
        public int? PercentageWorkingTimeDevotedToEnterprise { get; set; }
    }
}
