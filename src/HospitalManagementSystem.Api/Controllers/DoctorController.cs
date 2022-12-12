using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Api.Queries;
using MediatR;
using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Validators;

namespace HospitalManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : Controller
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IMediator _mediator;

        public DoctorController(IMediator mediator, ILogger<DoctorController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DoctorsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DoctorsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctors([FromQuery] DoctorsQuery query)
        {
            //var validation = await ModelValidation.ValidateModelAsync(ModelState, nameof(DoctorsQuery), HttpContext);

            //if (validation != null)
            //    return validation;

            var validator = new DoctorsQueryValidator();
            var result = validator.Validate(query);

            if (!result.IsValid) return BadRequest(result.Errors);

            try
            {
                var response = await _mediator.Send(query);

                if (!response.Doctors.Any()) return NotFound(response);

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error executing the query on doctors.");
                return StatusCode(500);
            }
        }
    }
}
