namespace iTender.Domain.Models
{
    public class CDPFinancialYearModel
    {
        public Guid Id { get; set; }
        public Guid CdpId { get; set; }
        public int FinancialYear { get; set; }
        public bool RemovedFromCdp { get; set; }
        public int StateCode { get; set; }
    }
}
