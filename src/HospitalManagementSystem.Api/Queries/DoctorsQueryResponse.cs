using HospitalManagementSystem.Api.Models;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQueryResponse
    {
        public List<Doctor> Doctors { get; set; }
        public DoctorsQueryDetail Detail { get; set; }
    }
}
