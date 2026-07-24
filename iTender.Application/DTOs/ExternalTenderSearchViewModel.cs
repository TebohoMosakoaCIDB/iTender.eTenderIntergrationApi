using Swashbuckle.AspNetCore.Annotations;

namespace iTender.Application.DTOs
{
    public class ExternalTenderSearchViewModel
    {
        [SwaggerParameter(Description = "Tender or EOI")]
        public string? NoticeType { get; set; }

        public string? TenderNumber { get; set; }

        [SwaggerParameter(Description = "Format: yyyy-MM-dd")]
        public DateTime? TendersClosingBefore { get; set; }

        public Guid? EmployerId { get; set; }

        public Guid? MetroDistrictId { get; set; }

        [SwaggerParameter(Description = "Format: yyyy-MM-dd")]
        public DateTime? DocumentsAvailableFrom { get; set; }

        public Guid? ProvinceId { get; set; }

        public Guid? ClassOfConstructionWorksId { get; set; }

        public Guid? AlternateClassOfConstructionWorksId { get; set; }

        public Guid? ClassOfConstructionWorksSubCategoryId { get; set; }

        public Guid? AlternateClassOfConstructionWorksSubCategoryId { get; set; }

        public int? DesignationId { get; set; }

        public int NumberOfResultsPerPage { get; set; } = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
