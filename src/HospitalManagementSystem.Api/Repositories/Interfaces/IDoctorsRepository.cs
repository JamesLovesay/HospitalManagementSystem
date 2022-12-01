using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;

namespace HospitalManagementSystem.Api.Repositories.Interfaces
{
    public interface IDoctorsRepository
    {
        Task<List<DoctorReadModel>> GetDoctors(DoctorsQueryModel query);
    }
}
