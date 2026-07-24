namespace iTender.Domain.Models
{
    public class CaseIncidentModel
    {
        public Guid Id { get; set; }
        public Guid? TenderId { get; set; }
        public Guid? ContractId { get; set; }
        public Guid? ServiceProviderId { get; set; }
        public int? CaseTypeCode { get; set; }
        public int? StatusReasonSubStatus { get; set; }
    }
}
