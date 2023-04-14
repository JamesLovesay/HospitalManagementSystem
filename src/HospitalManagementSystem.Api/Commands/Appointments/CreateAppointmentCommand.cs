using MediatR;

namespace HospitalManagementSystem.Api.Commands.Appointments;

public class CreateAppointmentCommand : IRequest<Guid>
{
    public string? Description { get; set; }
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string StartTime { get; set; } = DateTime.Now.ToString();
    public string EndTime { get; set; }
    public string? DoctorName { get; set; }
    public string? PatientName { get; set; }
}
