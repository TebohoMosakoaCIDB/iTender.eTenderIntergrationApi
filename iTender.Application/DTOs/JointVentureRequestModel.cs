namespace iTender.Application.DTOs
{
    public class JointVentureRequestModel
    {
        public int DesignationId { get; set; }
        public Guid ClassOfConstructionWorksId { get; set; }
        public List<string> ContractorCrsNumbers { get; set; }
    }
}
