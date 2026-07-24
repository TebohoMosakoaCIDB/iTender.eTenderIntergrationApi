namespace iTender.Domain.Models
{
    public class RecommendedGradeModel
    {
        public string Grade { get; set; } = string.Empty;
        public decimal? TenderFromValue { get; set; }
        public decimal? TenderToValue { get; set; }
        public decimal? AdminFee { get; set; }
        public decimal? AnnualFee { get; set; }
        public decimal? RegFee { get; set; }
        public string Result { get; set; }
    }
}
