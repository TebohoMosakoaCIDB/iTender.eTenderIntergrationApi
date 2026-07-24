using System.ComponentModel.DataAnnotations;

namespace iTender.Application.DTOs
{
    public class GradingDesignationCalculatorModel
    {
        public Guid ClassOfConstructionWorksId { get; set; }
        public decimal BestAnnualTurnover { get; set; }
        public decimal LargestContractValue { get; set; }
        public decimal ContractValue { get; set; }
        public decimal AvailableCapital { get; set; }
        public int NumberOfQualifiedProfessionals { get; set; }
        public bool ElectricalContractorsLicense { get; set; }
    }
}
