using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Api.Queries;
using MediatR;
using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Validators;
using Serilog;
using MongoDB.Bson;
using System.Diagnostics.Eventing.Reader;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Models;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders;

namespace HospitalManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : Controller
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;

        public DoctorsController(IMediator mediator, ILogger<DoctorsController> logger)
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

        [HttpGet("{doctorId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DoctorRecordQueryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctorById([FromRoute] DoctorRecordQuery query)
        {
            if (!ObjectId.TryParse(query.DoctorId, out ObjectId id)) return BadRequest("DoctorId is invalid. Please enter a valid object Id of length 24 characters.");

            try
            {
                var response = await _mediator.Send(query);
                if(response != null)
                {
                    if (response.NotFoundInReadStore())
                        return new NotFoundObjectResult(CommandResponse.From(query.DoctorId, $"Doctor not found for Id {query.DoctorId}"));

                    if (response.IsReady())
                    {
                        return Ok(response);
                    }
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting doctor Id = {doctorId}.", query.DoctorId);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand cmd, CancellationToken ct)
        {
            var validator = new CreateDoctorValidator();
            var result = validator.Validate(cmd);

            if (!result.IsValid) return BadRequest(result.Errors);

            try
            {
                var response = await _mediator.Send(cmd);

                return StatusCode(StatusCodes.Status201Created, $"Doctor created successfully. New ID = {response}");
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Error creating Doctor {cmd.Name}");
                return StatusCode(500);
            }
        }

        [HttpDelete("{doctorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveDoctorAsync([FromRoute] string doctorId)
        {
            if (!ObjectId.TryParse(doctorId, out ObjectId id)) return BadRequest("Doctor Id is invalid");

            var cmd = new DoctorDeleteCommand
            {
                DoctorId = doctorId,
            };

            try
            {
                if (await _mediator.Send(cmd))
                {
                    return Ok($"Delete command for doctor issued successfully. DoctorId={doctorId}");
                };
                return NotFound($"Doctor not found. DoctorId={doctorId}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Deleting Doctor. DoctorId={DoctorId}", cmd.DoctorId);
                return StatusCode(500);
            }
        }


        [HttpPut("{doctorId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutDoctorRate([FromBody] DoctorUpdateCommand cmd, [FromRoute] string doctorId)
        {
            if(!ObjectId.TryParse(doctorId, out ObjectId id))
                return BadRequest("Please enter a valid DoctorId");

            if(doctorId != cmd.DoctorId) return BadRequest("Invalid id provided.");

            var validator = new UpdateDoctorCommandValidator();
            var result = validator.Validate(cmd);

            if (!result.IsValid) return BadRequest(result.Errors);

            try
            {
                if (await _mediator.Send(cmd))
                {
                    return Ok($"Update command for doctor issued successfully. DoctorId={doctorId}");
                }
                return new NotFoundObjectResult(CommandResponse.From(cmd.DoctorId, $"Doctor not found for Id {cmd.DoctorId}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception updating doctor. DoctorId={doctorId}", doctorId);
                return StatusCode(500);
            }
        }
    }
}
