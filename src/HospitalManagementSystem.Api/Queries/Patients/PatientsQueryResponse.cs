using HospitalManagementSystem.Api.Models.Patients;

namespace HospitalManagementSystem.Api.Queries.Patients
{
    public class PatientsQueryResponse
    {
        public List<Patient> Patients { get; set; } = new List<Patient>();
        public PatientsQueryDetail Detail { get; set; } = new PatientsQueryDetail();
    }
}