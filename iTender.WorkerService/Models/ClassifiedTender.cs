using iTender.WorkerService.Enums;

namespace iTender.WorkerService.Models
{
    public class ClassifiedTender
    {
        public ExternalTender ExternalTender { get; set; }
        //public TenderModel? InternalTender { get; set; }
        public TenderComplianceStatus ComplianceStatus { get; set; }
        //public string? ComplianceReason { get; set; }
        public DateTime CheckedOn { get; set; }
        public string Source { get; set; } = string.Empty;
        public bool RequiresReview { get; set; }
    }
}
