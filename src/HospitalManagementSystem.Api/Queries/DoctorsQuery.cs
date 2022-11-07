using HospitalManagementSystem.Api.Models;
using MediatR;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQuery : IRequest<DoctorsQueryResponse>
    {
        public List<Guid>? DoctorId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public string? DoctorName { get; set; }
        public bool Active { get; set; }
        public DoctorSpecialism? Specialism { get; set; }
    }
}
