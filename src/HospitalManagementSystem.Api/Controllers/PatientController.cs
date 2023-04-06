using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Queries.Patients;
using HospitalManagementSystem.Api.Validators.Patients;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Globalization;

namespace HospitalManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : Controller
    {
        private readonly ILogger<PatientsController> _logger;
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator, ILogger<PatientsController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(PatientsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatients([FromQuery] PatientsQuery query)
        {
            var validator = new PatientsQueryValidator();
            var result = validator.Validate(query);

            if (!result.IsValid) return BadRequest(result.Errors);

            try
            {
                var response = await _mediator.Send(query);

                if (!response.Patients.Any()) return NotFound(response);

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error executing the query on patients.");
                return StatusCode(500);
            }
        }

        [HttpGet("{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientRecordQueryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientById([FromRoute] PatientRecordQuery query)
        {
            if (!ObjectId.TryParse(query.PatientId, out ObjectId id)) return BadRequest("PatientId is invalid. Please enter a valid object Id of length 24 characters.");

            try
            {
                var response = await _mediator.Send(query);
                if (response != null)
                {
                    if (response.NotFoundInReadStore())
                        return new NotFoundObjectResult(CommandResponse.From(query.PatientId, $"Patient not found for Id {query.PatientId}"));

                    if (response.IsReady())
                    {
                        return Ok(response);
                    }
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting patient with id {query.PatientId}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand cmd, CancellationToken ct)
        {
            var validator = new CreatePatientCommandValidator();
            var result = validator.Validate(cmd);

            if (!result.IsValid) return BadRequest(result.Errors);

            try
            {
                var patientId = await _mediator.Send(cmd);

                return StatusCode(StatusCodes.Status201Created, $"Doctor created successfully. New ID = {patientId}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error creating patient {cmd.FirstName} {cmd.LastName}");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CommandResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePatient([FromRoute] string patientId, [FromBody] UpdatePatientCommand cmd)
        {
            if (!ObjectId.TryParse(patientId, out ObjectId id))
                return BadRequest("Please enter a valid PatientId");

            if (patientId != cmd.PatientId) return BadRequest("Invalid ID providedd");

            var validator = new UpdatePatientCommandValidator();
            var result = validator.Validate(cmd);

            if (!result.IsValid) return BadRequest(result.Errors);

            try
            {
                if (await _mediator.Send(cmd))
                {
                    return Ok($"Update command for patient issued successfully. PatientId={patientId}");
                }
                return new NotFoundObjectResult(CommandResponse.From(cmd.PatientId, $"Patient not found for Id {cmd.PatientId}"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error updating patient with id {id}");
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePatient([FromRoute] string patientId)
        {
            if (!ObjectId.TryParse(patientId, out ObjectId id)) return BadRequest("Patient Id is invalid");

            try
            {
                if (await _mediator.Send(new DeletePatientCommand(patientId)))
                {
                    return Ok($"Delete command for patient issued successfully. PatientId={patientId}");
                };
                return NotFound($"Patient not found. DoctorId={patientId}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error deleting patient with id {id}");
                return StatusCode(500);
            }
        }
    }
}
