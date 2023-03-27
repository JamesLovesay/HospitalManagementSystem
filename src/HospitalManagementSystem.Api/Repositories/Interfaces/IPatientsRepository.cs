using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Patients;

namespace HospitalManagementSystem.Api.Repositories.Interfaces
{
    public interface IPatientsRepository
    {
        Task DeletePatient(string patientId);
        Task <PatientReadModel> GetPatientById(string patientId);
        Task<(List<PatientReadModel> patients, PatientsQueryDetail detail)> GetPatients(PatientsQueryModel query);
        Task UpsertPatient(PatientReadModel cmd);
    }
}