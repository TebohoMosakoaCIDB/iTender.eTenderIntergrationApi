using iTender.Application.Commands.Contact;
using iTender.Application.Commands.Tender;
using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Application.Queries.Contact;
using iTender.Application.Queries.Tender;
using iTender.Domain.Enums;
using iTender.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace iTender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Register Of Projects/Tenders")]
    public class TendersController : ControllerBase
    {
        private readonly GetTenderSummaryQueryHandler _summary;
        private readonly GetFilteredTendersQueryHandler _filter;
        private readonly GetTenderCountByProvinceQueryHandler _countByProvince;
        private readonly GetAdvancedFilteredTenderQueryHandler _filterAdvanced;
        private readonly GetTendersQueryHandler _getTendersHandler;
        private readonly GetContactsByTenderIdQueryHandler _contactHanlder;
        private readonly CreateContactCommandHandler _createContactHandler;
        private readonly CreateTenderCommandHandler _handler;
        private readonly GetTenderByIdQueryHandler _getByIdhandler;
        private readonly UpdateTenderCommandHandler _updatehandler;
        private readonly UpdateFullTenderCommandHandler _fullUpdatehandler;
        private readonly DeleteTenderCommandHandler _deletehandler;
        private readonly DeleteContactCommandHandler _deleteContactHandler;
        private readonly GetAllTendersQueryHandler _allTenderHandler;
        private readonly ILookupService _lookupService;
        public TendersController(GetTenderSummaryQueryHandler summary, 
            GetFilteredTendersQueryHandler filter, 
            GetTenderCountByProvinceQueryHandler countByProvince, 
            GetAdvancedFilteredTenderQueryHandler filterAdvanced, 
            GetTendersQueryHandler getTendersHandler, 
            GetContactsByTenderIdQueryHandler contactHanlder,
            CreateContactCommandHandler createContactHandler,
            CreateTenderCommandHandler handler,
            GetTenderByIdQueryHandler getByIdhandler,
            UpdateTenderCommandHandler updatehandler,
            DeleteTenderCommandHandler deletehandler,
            UpdateFullTenderCommandHandler fullUpdatehandler,
            DeleteContactCommandHandler deleteContactHandler,
            GetAllTendersQueryHandler allTenderHandler,
            ILookupService lookupService)
        {
            _summary = summary;
            _filter = filter;
            _countByProvince = countByProvince;
            _filterAdvanced = filterAdvanced;
            _getTendersHandler = getTendersHandler;
            _contactHanlder = contactHanlder;
            _lookupService = lookupService;
            _getByIdhandler = getByIdhandler;
            _updatehandler = updatehandler;
            _deletehandler = deletehandler;
            _fullUpdatehandler = fullUpdatehandler;
            _createContactHandler = createContactHandler;
            _deleteContactHandler = deleteContactHandler;
            _allTenderHandler = allTenderHandler;
            _handler = handler;
        }

        [HttpGet("TendersByProvince")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProvincialSummary(CancellationToken ct)
        {
            try
            {
                var result = await _countByProvince.Handle(ct);

                return Ok(result);
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
                        Message = "An unexpected error occurred while retrieving tenders."
                    });
            }
        }

        [HttpGet("TenderStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummary(CancellationToken ct)
        {
            try
            {
                var result = await _summary.Handle(ct);

                return Ok(result);
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
                        Message = "An unexpected error occurred while retrieving tenders."
                    });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TenderModel>), StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTenders(CancellationToken ct)
        {
            try
            {
                var query = new GetAllTendersQuery(ct);

                var results = _allTenderHandler.Handle(query);                

                foreach (var result in results)
                {
                    result.TypeOfContractName = _lookupService.GetTypeOfContractById(result.TypeOfContractId.Value).Result.Name;
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
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "An unexpected error occurred while retrieving tenders."
                    });
            }
        }

        [HttpGet("AdvancedTenderFilter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdvancedTenderFilter([FromQuery] AdvancedTenderSearchViewModel filter, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var query = new GetAdvancedFilteredTenderQuery(filter);

                var results = await _filterAdvanced.Handle(query, ct);

                foreach (var result in results.Items)
                {
                    //get contact by tender Id query 
                    var contactQuery = new GetContactsByTenderIdQuery(result.Id);
                    var contacts = await _contactHanlder.Handle(contactQuery, ct);
                    foreach (var contact in contacts)
                    {
                        ContactForTenderModel newContact = new ContactForTenderModel();
                        newContact.PersonToQuery = contact.FullName;
                        newContact.MobilePhoneNumber = contact.MobilePhone;
                        newContact.TelephoneNumber = contact.Telephone;
                        //newContact.FaxNumber = contact.Fax
                        newContact.Email = contact.Email;

                        result.ContactPerson.Add(newContact);
                    }
                    result.TypeOfContractName = _lookupService.GetTypeOfContractById(result.TypeOfContractId.Value).Result.Name;
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
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "An unexpected error occurred while retrieving tenders."
                    });
            }
        }

        [HttpGet("GetFilteredTenders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFilteredTenders([FromQuery] TenderFilterViewModel filter,[FromQuery] Guid employerId,[FromQuery] TenderType tenderType, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (employerId == Guid.Empty)
                {
                    return BadRequest("EmployerId is required.");
                }

                var query = new GetTendersQuery
                {
                    Filter = filter,
                    EmployerId = employerId,
                    TenderType = tenderType
                };

                var results = await _getTendersHandler.Handle(query, ct);

                foreach (var result in results.Items)
                {
                    //get contact by tender Id query 
                    var contactQuery = new GetContactsByTenderIdQuery(result.Id);
                    var contacts = await _contactHanlder.Handle(contactQuery, ct);
                    foreach (var contact in contacts)
                    {
                        ContactForTenderModel newContact = new ContactForTenderModel();
                        newContact.PersonToQuery = contact.FullName;
                        newContact.MobilePhoneNumber = contact.MobilePhone;
                        newContact.TelephoneNumber = contact.Telephone;
                        //newContact.FaxNumber = contact.Fax
                        newContact.Email = contact.Email;

                        result.ContactPerson.Add(newContact);
                    }
                    result.TypeOfContractName = _lookupService.GetTypeOfContractById(result.TypeOfContractId.Value).Result.Name;
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
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "An unexpected error occurred while retrieving tenders."
                    });
            }
        }

        [HttpPost("CreateTender")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateTenderModel model, CancellationToken ct)
        {
            try
            {
                if (model == null)
                    return BadRequest("Request body cannot be null.");                

                var id = await _handler.Handle(
                    new CreateTenderCommand(model),
                    ct);

                //update contact details
                if (model.ContactPerson.Count > 0)
                {
                    for (int i = 0; i < model.ContactPerson.Count; i++)
                    {
                        var contactCommand = new CreateContactCommand();
                        contactCommand.FirstName = model.ContactPerson[i].PersonToQuery;
                        contactCommand.LastName = model.ContactPerson[i].PersonToQuery;
                        contactCommand.Telephone = model.ContactPerson[i].TelephoneNumber;
                        contactCommand.MobilePhone = model.ContactPerson[i].MobilePhoneNumber;
                        contactCommand.Email = model.ContactPerson[i].Email;
                        contactCommand.ContactType = 100000001;
                        contactCommand.TenderId = id;

                        var handler = await _createContactHandler.Handle(contactCommand, ct);
                    }
                }

                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was cancelled.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log here if you have ILogger
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var query = new GetTenderByIdQuery(id);
                var result = await _getByIdhandler.Handle(query,ct);

                if (result == null)
                    return NotFound($"Tender with id '{id}' was not found.");

                var contactQuery = new GetContactsByTenderIdQuery(result.Id);
                var contacts = await _contactHanlder.Handle(contactQuery, ct);
                foreach (var contact in contacts)
                {
                    ContactForTenderModel newContact = new ContactForTenderModel();
                    newContact.PersonToQuery = contact.FullName;
                    newContact.MobilePhoneNumber = contact.MobilePhone;
                    newContact.TelephoneNumber = contact.Telephone;
                    newContact.FaxNumber = contact.FaxNumber;
                    newContact.Email = contact.Email;

                    result.ContactPerson.Add(newContact);
                }
                result.TypeOfContractName = _lookupService.GetTypeOfContractById(result.TypeOfContractId.Value).Result.Name;

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was cancelled.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log here if you have ILogger
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("ChangeTenderStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeTenderStatus(Guid tenderId, Guid employerId, TenderStatus status, CancellationToken ct)
        {
            try
            {
                var query = new GetTenderByIdQuery(tenderId);
                var result = await _getByIdhandler.Handle(query, ct);

                if (result == null) 
                {
                    return NotFound($"Tender with id '{tenderId}' was not found.");
                }

                if(result.EmployerId != employerId)
                {
                    return BadRequest("EmployerId does not match EmployerId used to create the Tender.");
                }

                if (status == TenderStatus.Draft)
                {
                    result.StatusCodeId = 1;
                }
                if (status == TenderStatus.Advertised)
                {
                    result.StatusCodeId = 100000000;
                    result.DateAdvertised = DateTime.Now;
                }
                if (status == TenderStatus.Cancelled)
                {
                    result.StatusCodeId = 100000001;
                    result.IsClosed = true;
                }

                var command = new UpdateTenderCommand(result);

                var id = await _updatehandler.Handle(command, ct);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error updating tender",
                    detail = ex.Message
                });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTenderModel model, CancellationToken ct)
        {
            if (id != model.Id)
                return BadRequest("Route ID and body ID mismatch.");

            //update contact details
            if (model.ContactPerson.Count > 0)
            {
                //delete contact first
                var contactQuery = new GetContactsByTenderIdQuery(model.Id);
                var contacts = await _contactHanlder.Handle(contactQuery, ct);
                for (int i = 0; i < model.ContactPerson.Count; i++)
                {
                    var deleteContactQuery = new DeleteContactCommand(contacts[i].Id);
                    await _deleteContactHandler.Handle(deleteContactQuery, ct);
                }                
            }

            //update contact details
            if (model.ContactPerson.Count > 0)
            {
                for (int i = 0; i < model.ContactPerson.Count; i++)
                {
                    var contactCommand = new CreateContactCommand();
                    contactCommand.FirstName = model.ContactPerson[i].PersonToQuery;
                    contactCommand.LastName = model.ContactPerson[i].PersonToQuery;
                    contactCommand.Telephone = model.ContactPerson[i].TelephoneNumber;
                    contactCommand.MobilePhone = model.ContactPerson[i].MobilePhoneNumber;
                    contactCommand.Email = model.ContactPerson[i].Email;
                    contactCommand.ContactType = 100000001;
                    contactCommand.TenderId = id;

                    var handler = await _createContactHandler.Handle(contactCommand, ct);
                }
            }

            var command = new UpdateFullTenderCommand(model);

            var result = await _fullUpdatehandler.Handle(command, ct);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Invalid Id provided");
                }
                var command = new DeleteTenderCommand(id);

                await _deletehandler.Handle(command, ct);

                return Ok(new { Message = "Tender deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error deleting tender",
                    detail = ex.Message
                });
            }
        }
    }
}