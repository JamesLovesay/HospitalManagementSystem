using HospitalManagementSystem.Api.Commands.Appointments;
using HospitalManagementSystem.Api.Events.Appointments;
using HospitalManagementSystem.Api.Models.Appointments;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Handlers.Appointments;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _repository;
    private readonly IMediator _mediator;

    public CreateAppointmentCommandHandler(IAppointmentRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand command, CancellationToken cancellationToken)
    {
        var appointment = new Appointment(Guid.NewGuid(), command.Description, command.PatientId, command.StartTime, command.EndTime, command.DoctorName, command.PatientName, command.DoctorId);

        await _repository.AddAppointment(appointment);

        await _mediator.Publish(new AppointmentCreatedEvent(appointment), cancellationToken);

        return appointment.Id;
    }
}
