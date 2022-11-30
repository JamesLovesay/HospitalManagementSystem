using HospitalManagementSystem.Api.Models;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQueryResponse
    {
        public List<DoctorReadModel> Doctors { get; set; } = new List<DoctorReadModel>();
    }
}
