using iTender.Application.DTOs;

namespace iTender.Application.Queries.ContractorDevelopmentProgramme
{
    public class GetCDPByEmployerIdQuery
    {
        public CDPViewModel Filter { get; set; }

        public GetCDPByEmployerIdQuery(CDPViewModel filter)
        {
            Filter = filter;
        }
    }
}
