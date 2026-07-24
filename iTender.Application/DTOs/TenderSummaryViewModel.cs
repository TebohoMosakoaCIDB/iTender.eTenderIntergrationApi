namespace iTender.Application.DTOs
{
    public class TenderSummaryViewModel
    {
        //public List<string> Grades { get; set; } = new(); // 1–9
        public List<string> Designations { get; set; } = new();
        public List<ClassOfWorkSummary> Rows { get; set; } = new();
    }

    public class ClassOfWorkSummary
    {
        public Guid ClassOfWorkId { get; set; }
        public string ClassOfWorkName { get; set; }

        public Dictionary<int, int> DesignationIdCounts { get; set; } = new(); // Grade -> Count
    }
}
