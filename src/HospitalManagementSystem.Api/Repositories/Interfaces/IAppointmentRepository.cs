using HospitalManagementSystem.Api.Models.Appointments;

namespace HospitalManagementSystem.Api.Repositories.Interfaces;

public interface IAppointmentRepository
{
    Task<Guid> AddAppointment(Appointment appointment);
    Task<AppointmentReadModel?> GetAppointmentById(int id);
}