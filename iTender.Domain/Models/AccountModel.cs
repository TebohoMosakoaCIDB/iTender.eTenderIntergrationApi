namespace iTender.Domain.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public string? CrsNumber { get; set; }
        public Guid? PrimaryContactId { get; set;}
        public string? Name { get; set; }
        public string? TradingAs { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? ShortName { get; set; }
        public Guid? ProvinceId { get; set; }
        public string? PostalCode { get; set; }
        public decimal? CurrentBalance { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsSanctioned { get; set; }
        public string? CsdNumber { get; set; }
        public int? StatusCode { get; set; }
        public Guid? Grade { get; set; }
        public Guid? Enterprise { get; set; }
        public string? EnterpriseRegistrationNumber { get; set; }
        public DateTime? AnnualUpdateDueDate { get; set; }
        public DateTime? RenewalDueDate { get; set; }
        public string? CurrentContractorGradingDesignation { get; set; }
        public string? CurrentContractorGrade { get; set; }
        public DateTime? DateEnterpriseRegistered { get; set; }
        public DateTime? DateOperationsStarted { get; set; }
        public string? SuspensionReasonAnnualUpdate { get; set; }
        public string? Type { get; set; }
        public string? EnterpriseType { get; set; }
    }
}
