using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQueryDetail
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public List<string>? Status { get; set; }
        public List<string>? Specialisms { get; set; }
        public List<string>? DoctorId { get; set; }
        public string? Name { get; set; }

    }
}