using iTender.Domain.Enums;

namespace iTender.Application.DTOs
{
    public class TenderFilterViewModel
    {
        public string? CIDBRefenceNumber { get; set; }
        public string? TenderNumber { get; set; }
        public string? Description { get; set; }        
        public UserFilter? User { get; set; }
        public Guid? UserId { get; set; }
        public TenderStatusFilter? Status { get; set; }
        public int PageSise { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
