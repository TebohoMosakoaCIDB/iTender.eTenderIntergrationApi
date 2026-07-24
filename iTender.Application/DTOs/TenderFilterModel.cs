namespace iTender.Application.DTOs
{
    public class TenderFilterModel
    {
        public Guid? ProvinceId { get; set; }
        public Guid? ClassOfConstructionWorksId { get; set; }
        public Guid? AlternateClassOfConstructionWorksId { get; set; }
        public int? DesignationId { get; set; }

        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
