using MediatR;

namespace HospitalManagementSystem.Api.Queries.Appointments;

public class AppointmentRecordQuery : IRequest<AppointmentRecordQueryResponse>
{
    public int Id { get; set; }
}
