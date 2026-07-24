using iTender.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace iTender.Application.DTOs
{
    public class JVGradingDesignationModel
    {
        public bool Success { get; set; }        
        public string RecommendedGrade { get; set; }
        public string DateCalculated { get; set; }
        public TenderValueRangeModel Designation { get; set; }
        public ClassOfConstructionWorkModel ClassOfWork { get; set; }
        public List<ContractorModel> Contractors { get; set; }
    }
}
