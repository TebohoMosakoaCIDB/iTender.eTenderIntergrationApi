namespace iTender.Application.DTOs
{
    public class RecommendedGradeModel
    {
        public string Grade { get; set; } = string.Empty;
        public decimal? TenderFromValue { get; set; }
        public decimal? TenderToValue { get; set; }
        public decimal? AdminFee { get; set; }
        public decimal? AnnualFee { get; set; }
        public decimal RegFee { get; set; }
        public string Message { get; set; }
    }
}
