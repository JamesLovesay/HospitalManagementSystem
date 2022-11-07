using HospitalManagementSystem.Api.Models;
using MediatR;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQuery : IRequest<DoctorsQueryResponse>
    {
        public List<Guid>? DoctorIds { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public string? DoctorName { get; set; }
        public List<DoctorStatus>? Statuses { get; set; }
        public List<DoctorSpecialism>? Specialism { get; set; }
    }
}
