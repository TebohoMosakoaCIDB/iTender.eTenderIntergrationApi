using iTender.Application.DTOs;
using iTender.Application.Queries.CdpSubmissions;
using iTender.Application.Queries.Contractor;
using iTender.Application.Queries.ContractorDevelopmentProgramme;
using Microsoft.AspNetCore.Mvc;

namespace iTender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Contractor Development Programmes")]
    public class ContractorDevelopmentProgrammeController : ControllerBase
    {
        private readonly GetCDPByEmployerIdQueryHandler _handler;
        private readonly GetCdpSubmissionsByCdpIdQueryHandler _cdpSubmissionHandler;
        private readonly GetContractorByIdQueryHandler _contractorHandler;

        public ContractorDevelopmentProgrammeController(GetCDPByEmployerIdQueryHandler handler, GetCdpSubmissionsByCdpIdQueryHandler cdpSubmissionHandler, GetContractorByIdQueryHandler contractorHandler)
        {
            _handler = handler;
            _contractorHandler = contractorHandler;
            _cdpSubmissionHandler = cdpSubmissionHandler;
        }

        [HttpGet("GetContractorDevelopmentProgrammesByEmployerId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByEmployerId([FromQuery] CDPViewModel filter, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (filter.EmployerId == Guid.Empty)
                {
                    return BadRequest("EmployerId is required.");
                }

                var query = new GetCDPByEmployerIdQuery(filter);
                var results = await _handler.Handle(query, ct);

                foreach (var result in results.Items) 
                {
                    var submissionQuery = new GetCdpSubmissionsByCdpIdQuery(result.Id);
                    var submissions = await _cdpSubmissionHandler.Handle(submissionQuery, ct);

                    foreach (var submission in submissions) 
                    {
                        var contractorQuery = new GetContractorByIdQuery((Guid)submission.ContractorId);
                        result.ContractorSubmissions.Add(await _contractorHandler.Handle(contractorQuery, ct));
                        result.SubmissionCount = result.ContractorSubmissions.Count;
                    }
                }                

                return Ok(results);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(
                    StatusCodes.Status499ClientClosedRequest,
                    new
                    {
                        Message = "The request was cancelled."
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "An unexpected error occurred while retrieving Contractor Development Programmes."
                    });
            }
        }
    }
}
