using iTender.Domain.Enums;

namespace iTender.Application.DTOs
{
    public class ContractFilterViewModel
    {
        public string? CIDBRefenceNumber { get; set; }
        public string? ContractNumber { get; set; }
        public string? ContractTitle { get; set; }
        public string? ContractDescription { get; set; }
        public ContractStatusFilter? Status { get; set; }
        public Guid? ProvinceId { get; set; }
        public UserFilter? User { get; set; }
        public Guid? UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSise { get; set; } = 10;
    }
}
