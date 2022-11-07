using HospitalManagementSystem.Api.Models;

namespace HospitalManagementSystem.Api.Repositories.Interfaces
{
    public interface IDoctorsRepository
    {
        Task<List<Doctor>> GetDoctors(DoctorsQueryModel query);
    }
}
