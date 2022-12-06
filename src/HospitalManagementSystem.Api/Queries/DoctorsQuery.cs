using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models;
using MediatR;
using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQuery : IRequest<DoctorsQueryResponse>
    {
        public List<ObjectId>? DoctorIds { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public string? DoctorName { get; set; }
        public List<string> Statuses { get; set; }
        public List<string> Specialisms { get; set; }

    }
}
