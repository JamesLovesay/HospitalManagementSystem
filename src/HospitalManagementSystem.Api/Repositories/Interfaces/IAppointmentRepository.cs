using HospitalManagementSystem.Api.Models.Appointments;

namespace HospitalManagementSystem.Api.Repositories.Interfaces;

public interface IAppointmentRepository
{
    Task<AppointmentReadModel?> GetAppointmentById(int id);
}