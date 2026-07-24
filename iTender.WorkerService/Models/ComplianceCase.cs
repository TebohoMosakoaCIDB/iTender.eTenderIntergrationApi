using iTender.WorkerService.Enums;

namespace iTender.WorkerService.Models
{
    public class ComplianceCase
    {
        public Guid Id { get; set; }
        public string TenderNumber { get; set; } = string.Empty;
        public string TenderTitle { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public ComplianceStatus Status { get; set; }

        public DateTime DetectedOn { get; set; }
        public DateTime LastCheckedOn { get; set; }

        public DateTime? LetterSentOn { get; set; }
        public DateTime? ResponseDueOn { get; set; }

        public bool LetterSent { get; set; }
    }
}
