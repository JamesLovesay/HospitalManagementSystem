using HospitalManagementSystem.Api.Queries;

namespace HospitalManagementSystem.Api.Models
{
    public class DoctorsQueryModel
    {
        public List<Guid>? DoctorIds { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<DoctorSpecialism>? Specialisms { get; set; }
        public List<DoctorStatus>? Status { get; set; }
        public string? Name { get; set; }

        public DoctorsQueryModel()
        {
        }

        public DoctorsQueryModel(DoctorsQuery query)
        {
            DoctorIds = query.DoctorIds == null ? null : new List<Guid>(query.DoctorIds);
            Page = query.Page;
            PageSize = query.PageSize;
            Specialisms = query.Specialisms == null ? null : new List<DoctorSpecialism>(query.Specialisms);
            Status = query.Statuses == null ? null : new List<DoctorStatus>(query.Statuses);
            Name = query.DoctorName == null ? null : query.DoctorName;
        }
    }
}
