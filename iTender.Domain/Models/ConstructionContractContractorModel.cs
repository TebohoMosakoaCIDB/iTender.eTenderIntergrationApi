namespace iTender.Domain.Models
{
    public class ConstructionContractContractorModel
    {
        public Guid Id { get; set; }
        public Guid? ConstructionContractId { get; set; }
        public Guid? ContractorId { get; set; }
        public bool? ValidCidbRegistration { get; set; }
        public string? Enterprisename { get; set; }
    }
}
