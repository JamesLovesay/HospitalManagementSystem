using MediatR;

namespace HospitalManagementSystem.Api.Queries.Patients
{
    public class PatientRecordQuery : IRequest<PatientRecordQueryResponse>
    {
        public string? PatientId { get; set; }
    }
}