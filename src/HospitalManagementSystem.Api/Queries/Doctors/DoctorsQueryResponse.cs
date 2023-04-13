using HospitalManagementSystem.Api.Models.Doctors;

namespace HospitalManagementSystem.Api.Queries.Doctors
{
    public class DoctorsQueryResponse
    {
        public List<Doctor> Doctors { get; set; }
        public DoctorsQueryDetail Detail { get; set; }
    }
}
