using AutoMapper;
using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Application.Queries.Contact;
using iTender.Application.Queries.Contractor;
using iTender.Application.Queries.ContractorGrade;
using iTender.Application.Queries.FinancialStatement;
using iTender.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace iTender.Api.Controllers
{
    [Route("api/contractors")]
    [ApiController]
    [Tags("Register Of Contractors")]
    public class ContractorController : ControllerBase
    {
        private readonly ILookupService _lookupService;
        private readonly IMapper _mapper;

        public ContractorController(ILookupService lookupService, IMapper mapper)
        {
            _mapper = mapper;
            _lookupService = lookupService;
        }               

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PagedResult<TenderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id, [FromServices] GetContractorByIdQueryHandler handler, [FromServices] GetContractorGradesQueryHandler gradesHandler,[FromServices] GetFinancialStatementsByContractorHandler fsHandler, [FromServices] GetContactByIdQueryHandler contactHandler,CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                var result = await handler.Handle(
                    new GetContractorByIdQuery(id),
                    ct);

                if (result == null)
                    return NotFound($"Contractor with id {id} was not found.");

                var gradesQuery = new GetContractorGradesQuery(result.Id, null, null);
                result.Grades = await gradesHandler.Handle(gradesQuery, ct) ?? new List<ContractorGradeModel>();

                var fsQuery = new GetFinancialStatementsByContractorQuery(result.Id);
                var statements = await fsHandler.Handle(fsQuery, ct) ?? new List<FinancialStatementModel>();                

                var latestStatement = statements
                    .OrderByDescending(x => x.Year)
                    .FirstOrDefault();

                result.AnnualTurnOver = latestStatement?.TurnoverInclVat ?? 0;
                result.NetAssetValue = latestStatement?.NetAssetValue ?? 0;

                var cQuery = new GetContactByIdQuery(result.PrimaryContactId.Value);
                var contact = await contactHandler.Handle(cQuery, ct);

                result.ContactPersonName = contact.LastName + " " + contact.FirstName;
                result.ContactPersonEmailAddress = contact.Email;
                result.ContactPersonTelephone = contact.Telephone;
                result.ContactPersonMobileNumber = contact.MobilePhone;

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while processing the request.");
            }
        }

        [HttpGet("GetContractorByCrsNumber/{crsNumber}")]
        [ProducesResponseType(typeof(PagedResult<TenderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCrsNumber(string crsNumber, [FromServices] GetContractorByCrsNumberQueryHandler handler, [FromServices] GetContractorGradesQueryHandler gradesHandler, [FromServices] GetFinancialStatementsByContractorHandler fsHandler, [FromServices] GetContactByIdQueryHandler contactHandler, CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(crsNumber))
                    return BadRequest("CRS number is required.");

                var result = await handler.Handle(
                    new GetContractorByCrsNumberQuery(crsNumber),
                    ct);

                if (result == null)
                    return NotFound($"Contractor with CRS number '{crsNumber}' was not found.");

                // Grades
                var gradesQuery = new GetContractorGradesQuery(result.Id, null, null);
                result.Grades = await gradesHandler.Handle(gradesQuery, ct)
                                ?? new List<ContractorGradeModel>();

                // Financial statements
                var fsQuery = new GetFinancialStatementsByContractorQuery(result.Id);
                var statements = await fsHandler.Handle(fsQuery, ct)
                                  ?? new List<FinancialStatementModel>();

                var latest = statements
                    .OrderByDescending(x => x.Year)
                    .FirstOrDefault();

                result.AnnualTurnOver = latest?.TurnoverInclVat ?? 0;
                result.NetAssetValue = latest?.NetAssetValue ?? 0;

                var cQuery = new GetContactByIdQuery(result.PrimaryContactId.Value);
                var contact = await contactHandler.Handle(cQuery, ct);

                result.ContactPersonName = contact.LastName + " " + contact.FirstName;
                result.ContactPersonEmailAddress = contact.Email;
                result.ContactPersonTelephone = contact.Telephone;
                result.ContactPersonMobileNumber = contact.MobilePhone;

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception ex)
            {
                // log here ideally (ILogger)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while processing the request.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TenderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] ContractorFilterRequestModel filter, [FromServices] GetContractorsQueryHandler handler,[FromServices] GetContractorGradesQueryHandler gradesHandler, [FromServices] GetContactByIdQueryHandler contactHandler, CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                if (filter == null)
                    return BadRequest("Filter is required.");

                var filterModel = _mapper.Map<ContractorFilterModel>(filter);

                if (filter.DesignationId != 0)
                {
                    filterModel.ApprovedGrade = await _lookupService
                        .ResolveApprovedGradeFromTenderRangeAsync(filter.DesignationId);
                }

                var result = await handler.Handle(
                    new GetContractorsQuery(filterModel),
                    ct);

                if (result?.Items == null || !result.Items.Any())
                    return Ok(result);

                // ⚠️ PERFORMANCE NOTE: this is N+1 (will be slow on large datasets)
                foreach (var item in result.Items)
                {
                    var grades = await gradesHandler.Handle(
                        new GetContractorGradesQuery(item.Id, null, null),
                        ct);

                    item.Grades = grades ?? new List<ContractorGradeModel>();

                    var cQuery = new GetContactByIdQuery(item.PrimaryContactId.Value);
                    var contact = await contactHandler.Handle(cQuery, ct);

                    item.ContactPersonName = contact.LastName + " " + contact.FirstName;
                    item.ContactPersonEmailAddress = contact.Email;
                    item.ContactPersonTelephone = contact.Telephone;
                    item.ContactPersonMobileNumber = contact.MobilePhone;
                }

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception ex)
            {
                // log exception here
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while retrieving contractors.");
            }
        }
    }
}
