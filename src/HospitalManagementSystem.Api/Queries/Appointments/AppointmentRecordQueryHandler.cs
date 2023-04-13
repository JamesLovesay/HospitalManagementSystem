using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Queries.Appointments;

public class AppointmentRecordQueryHandler : IRequestHandler<AppointmentRecordQuery, AppointmentRecordQueryResponse>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentRecordQueryHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<AppointmentRecordQueryResponse> Handle(AppointmentRecordQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetAppointmentById(request.Id);

        return new AppointmentRecordQueryResponse { Appointment = appointment };
    }
}
