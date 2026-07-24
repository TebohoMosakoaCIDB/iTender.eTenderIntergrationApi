using iTender.Application.DTOs;
using iTender.Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace iTender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Register Of Contractors")]
    public class ContractorGradingCalculatorController : ControllerBase
    {
        private readonly GradingDesignationCalcUtil _utility;

        public ContractorGradingCalculatorController(GradingDesignationCalcUtil utility)
        {
            _utility = utility;
        }

        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] GradingDesignationCalculatorModel model, CancellationToken ct)
        {
            try
            {
                if (model == null)
                    return BadRequest("Request body cannot be null.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _utility.GetRecommendedGradeAsync(model);

                if (result == null)
                    return NotFound("Could not calculate recommended grade.");

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was cancelled.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while calculating the recommended grade."
                });
            }
        }
    }
}
