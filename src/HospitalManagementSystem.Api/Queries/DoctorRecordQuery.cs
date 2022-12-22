using MediatR;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorRecordQuery : IRequest<DoctorRecordQueryResponse>
    {
        public string? DoctorId { get; set; }
    }
}