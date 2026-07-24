using iTender.Application.DTOs;
using iTender.Application.Queries.Contract;
using iTender.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace iTender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Register Of Projects/Awards")]
    public class ContractsController : ControllerBase
    {
        private readonly CheckForDuplicateContractNumbersQueryHandler _duplicateHandler;
        private readonly GetContractsByContractNumberQueryHandler _byNumberHandler;
        private readonly GetContractsAwardedQueryHandler _awardedHandler;
        private readonly GetContractByIdQueryHandler _byIdHandler;
        private readonly AdvancedAwardSearchQueryHandler _advancedSearchHandler;
        private readonly GetContractsQueryHandler _contractsHandler;
        public ContractsController(
        CheckForDuplicateContractNumbersQueryHandler duplicateHandler,
        GetContractsByContractNumberQueryHandler byNumberHandler,
        GetContractsAwardedQueryHandler awardedHandler,
        GetContractByIdQueryHandler byIdHandler,
        AdvancedAwardSearchQueryHandler advancedSearchHandler,
        GetContractsQueryHandler contractsHandler)
        {
            _contractsHandler = contractsHandler;
            _duplicateHandler = duplicateHandler;
            _byNumberHandler = byNumberHandler;
            _awardedHandler = awardedHandler;
            _byIdHandler = byIdHandler;
            _advancedSearchHandler = advancedSearchHandler;
        }

        [HttpGet("GetContracts")]
        [ProducesResponseType(typeof(PagedResult<ContractModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContracts([FromQuery] ContractFilterViewModel filter, [FromQuery] Guid employerId, CancellationToken ct)
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

                var query = new GetContractsQuery
                {
                    Filter = filter,
                    EmployerId = employerId
                };

                var result = await _contractsHandler.Handle(query, ct);

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

        [HttpGet("GetAwardedContracts")]
        [ProducesResponseType(typeof(PagedResult<ContractModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAwardedContracts([FromQuery] Guid employerId, CancellationToken ct)
        {
            try
            {
                var query = new GetContractsAwardedQuery(employerId, true);

                var result = await _awardedHandler.Handle(query, ct);

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

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PagedResult<ContractModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByContractId(Guid id, CancellationToken ct)
        {
            try
            {
                var query = new GetContractByIdQuery(id);

                var result = await _byIdHandler.Handle(query, ct);

                if (result == null)
                    return NotFound();

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

        [HttpGet("AdvancedAwardSearch")]
        [ProducesResponseType(typeof(PagedResult<ContractModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdvancedAwardSearch([FromQuery] AdvancedAwardSearchModel filter, CancellationToken ct)
        {
            try
            {
                var query = new AdvancedAwardSearchQuery(filter);

                var result = await _advancedSearchHandler.Handle(query, ct);

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
    }
}
