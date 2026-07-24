namespace iTender.Domain.Models
{
    public class ApplicationModel
    {
        public Guid Id { get; set; }
        public string? ApplicationNumber { get; set; }
        public string? Type { get; set; }
        public int? StatusCode { get; set; }
        public Guid ContractorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ActivationDate { get; set; }
        public int StateCode { get; set; }
        public bool ContractorPotentiallyEmerging { get; set; }
    }
}
