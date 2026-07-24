using Swashbuckle.AspNetCore.Annotations;

namespace iTender.Application.DTOs
{
    public class AdvancedAwardSearchModel
    {
        public string? TenderNumber { get; set; }
        public Guid? EmployerId { get; set; }
        public Guid? ProvinceID { get; set; }
        public Guid? MetroDistMuniID { get; set; }
        public Guid? CoCW { get; set; }
        public int? TenderValueRange { get; set; }
        public string? ContractTitle { get; set; }
        public string? ContractDescr { get; set; }
        public string? ContractNumber { get; set; }
        [SwaggerParameter(Description = "Format: yyyy-MM-dd")]
        public string? AwardFromDate { get; set; }
        [SwaggerParameter(Description = "Format: yyyy-MM-dd")]
        public string? AwardBeforeDate { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool? Isclosed { get; set; }

        public string[]? SortBycolumn { get; set; }
    }
}
