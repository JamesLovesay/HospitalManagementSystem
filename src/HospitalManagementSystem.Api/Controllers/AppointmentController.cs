using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Queries.Appointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace HospitalManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : Controller
{
    private readonly ILogger<AppointmentsController> _logger;
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator, ILogger<AppointmentsController> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentRecordQueryResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CommandResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        if (!Regex.IsMatch(id.ToString(), @"^\d+$"))
        {
            return BadRequest();
        }

        try
        {
            var query = new AppointmentRecordQuery { Id = id };
            var appointment = await _mediator.Send(query);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error retrieving appointment with id: {id}");
            return StatusCode(500);
        }
    }

    //[HttpPost]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
    //{
    //    try
    //    {
    //        var appointmentId = await _mediator.Send(command);

    //        return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentId }, null);
    //    }
    //    catch (Exception e)
    //    {
    //        _logger.LogError(e, "Error creating appointment.");
    //        return StatusCode(500);
    //    }
    //}

    //[HttpPut("{id}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentCommand command)
    //{
    //    try
    //    {
    //        if (id != command.Id)
    //        {
    //            return BadRequest();
    //        }

    //        var appointmentExists = await _mediator.Send(new AppointmentExistsQuery { Id = id });

    //        if (!appointmentExists)
    //        {
    //            return NotFound();
    //        }

    //        await _mediator.Send(command);

    //        return NoContent();
    //    }
    //    catch (Exception e)
    //    {
    //        _logger.LogError(e, $"Error updating appointment with id: {id}");
    //        return StatusCode(500);
    //    }
    //}

    //[HttpDelete("{id}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public async Task<IActionResult> CancelAppointment(int id)
    //{
    //    try
    //    {
    //        var appointmentExists = await _mediator.Send(new AppointmentExistsQuery { Id = id });

    //        if (!appointmentExists)
    //        {
    //            return NotFound();
    //        }

    //        await _mediator.Send(new CancelAppointmentCommand { Id = id });

    //        return NoContent();
    //    }
    //    catch (Exception e)
    //    {
    //        _logger.LogError(e, $"Error cancelling appointment with id: {id}");
    //        return StatusCode(500);
    //    }
    //}

}
