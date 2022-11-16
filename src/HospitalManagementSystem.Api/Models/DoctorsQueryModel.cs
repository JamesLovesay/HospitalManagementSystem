using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Queries;

namespace HospitalManagementSystem.Api.Models
{
    public class DoctorsQueryModel
    {
        public List<Guid>? DoctorIds { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<string> Specialisms { get; set; }
        public List<string>? Status { get; set; }
        public string? Name { get; set; }
        public string SortDirection { get; set; }
        public string SortBy { get; set; }

        public DoctorsQueryModel()
        {
            Specialisms = new List<string>();
            SortDirection = QueryHelper.SortDescending;
            SortBy = QueryHelper.DoctorSortableFields[0];
        }

        public DoctorsQueryModel(DoctorsQuery query)
        {
            DoctorIds = query.DoctorIds == null ? null : new List<Guid>(query.DoctorIds);
            Page = query.Page;
            PageSize = query.PageSize;
            Specialisms = query.Specialisms == null ? new List<string>() : query.Specialisms;
            Status = query.Statuses;
            Name = query.DoctorName == null ? null : query.DoctorName;
            SortDirection = query.SortDirection;
            SortBy = query.SortBy;
        }
    }
}
