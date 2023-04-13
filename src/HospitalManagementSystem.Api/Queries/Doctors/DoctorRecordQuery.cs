using MediatR;

namespace HospitalManagementSystem.Api.Queries.Doctors
{
    public class DoctorRecordQuery : IRequest<DoctorRecordQueryResponse>
    {
        public string? DoctorId { get; set; }
    }
}