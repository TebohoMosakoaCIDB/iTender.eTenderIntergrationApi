using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Application.Providers;
using iTender.Application.Queries.Contact;
using iTender.Application.Queries.Tender;
using iTender.Domain.Models;
using iTender.Infrastructure.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace iTender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETenderController : ControllerBase
    {

        private readonly GetContactsByTenderIdQueryHandler _contactHanlder;
        private readonly GetAdvancedFilteredTenderQueryHandler _filterAdvanced;
        private readonly ILookupService _lookupService;
        private readonly IETenderApiProvider _provider;

        public ETenderController(ILookupService? lookupService, GetContactsByTenderIdQueryHandler? contactHandler, GetAdvancedFilteredTenderQueryHandler? filterAdvanced, IETenderApiProvider? provider)
        {
            _provider = provider;
            _lookupService = lookupService;
            _contactHanlder = contactHandler;
            _filterAdvanced = filterAdvanced;
        }

        [HttpGet("eTenderFormat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExternalTenderFilter([FromQuery] ExternalTenderSearchViewModel filter, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Reuse the existing query
                var internalFilter = new AdvancedTenderSearchViewModel
                {
                    NoticeType = filter.NoticeType,
                    TenderNumber = filter.TenderNumber,
                    TendersClosingBefore = filter.TendersClosingBefore,
                    EmployerId = filter.EmployerId,
                    MetroDistrictId = filter.MetroDistrictId,
                    DocumentsAvailableFrom = filter.DocumentsAvailableFrom,
                    ProvinceId = filter.ProvinceId,
                    ClassOfConstructionWorksId = filter.ClassOfConstructionWorksId,
                    AlternateClassOfConstructionWorksId = filter.AlternateClassOfConstructionWorksId,
                    ClassOfConstructionWorksSubCategoryId = filter.ClassOfConstructionWorksSubCategoryId,
                    AlternateClassOfConstructionWorksSubCategoryId = filter.AlternateClassOfConstructionWorksSubCategoryId,
                    DesignationId = filter.DesignationId,
                    numberOfResultsPerPage = filter.NumberOfResultsPerPage,
                    PageNumber = filter.PageNumber,
                    PageSise = filter.PageSize
                };

                var query = new GetAdvancedFilteredTenderQuery(internalFilter);

                var results = await _filterAdvanced.Handle(query, ct);

                var externalTenders = new List<ExternalTenderModel>();

                foreach (var result in results.Items)
                {
                    // Populate contacts
                    var contactQuery = new GetContactsByTenderIdQuery(result.Id);
                    var contacts = await _contactHanlder.Handle(contactQuery, ct);

                    foreach (var contact in contacts)
                    {
                        result.ContactPerson.Add(new ContactForTenderModel
                        {
                            PersonToQuery = contact.FullName,
                            MobilePhoneNumber = contact.MobilePhone,
                            TelephoneNumber = contact.Telephone,
                            Email = contact.Email
                        });
                    }

                    // Populate lookup
                    result.TypeOfContractName =
                        (await _lookupService.GetTypeOfContractById(result.TypeOfContractId!.Value)).Name;

                    // Convert to external model
                    externalTenders.Add(result.ToExternal());
                }

                return Ok(externalTenders);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(StatusCodes.Status499ClientClosedRequest, new
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
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An unexpected error occurred while retrieving tenders."
                });
            }
        }

        [HttpGet("CIDBFormat")]
        public async Task<IActionResult> GetInternalFormat(CancellationToken ct)
        {
            var external = await _provider.GetTendersAsync(ct);

            var internalTenders = external
                .Select(x => x.ToInternal())
                .ToList();

            return Ok(internalTenders);
        }
    }
}
