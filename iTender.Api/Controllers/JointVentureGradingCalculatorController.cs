using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace iTender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Register Of Contractors")]
    public class JointVentureGradingCalculatorController : ControllerBase
    {
        private readonly ILookupService _lookupService;
        private readonly IJointVentureRepository _jointVentureRepository;

        public JointVentureGradingCalculatorController(ILookupService lookupService, IJointVentureRepository jointVentureRepository)
        {
            _lookupService = lookupService;
            _jointVentureRepository = jointVentureRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Calculate([FromQuery] JointVentureRequestModel model, CancellationToken ct)
        {
            try
            {
                if (model == null)
                    return BadRequest("Invalid Request Parameters.");

                if (model.ContractorCrsNumbers.Count <= 1)
                    return BadRequest("Please ensure that more than one Contractor CRS Number is captured for JV Calculation.");

                if (model.ContractorCrsNumbers.Count >= 5)
                    return BadRequest("A Joint Venture cannot have more than 5 contractors.");

                JVGradingDesignationModel result = new JVGradingDesignationModel();
                result.Designation = _lookupService.GetTenderValueRangeById(model.DesignationId).Result;
                result.ClassOfWork = _lookupService.GetClassOfConstructionWorkByIdAsync(model.ClassOfConstructionWorksId).Result;

                result.Contractors = new List<ContractorModel>();

                foreach (var contractor in model.ContractorCrsNumbers)
                {
                    result.Contractors.Add(new ContractorModel
                    {
                        CrsNumber = contractor
                    });
                }
                result = await _jointVentureRepository.GetRecommendedGrade(result);

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was cancelled.");
            }
            catch (Exception)
            {
                // log exception here
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
