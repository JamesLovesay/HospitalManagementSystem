using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;

namespace HospitalManagementSystem.Api.Repositories.Interfaces
{
    public interface IDoctorsRepository
    {
        Task<(List<DoctorReadModel> doctors, DoctorsQueryDetail detail)> GetDoctors(DoctorsQueryModel query);
    }
}
