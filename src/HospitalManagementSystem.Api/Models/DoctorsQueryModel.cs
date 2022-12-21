using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Queries;
using MediatR;
using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Models
{
    public class DoctorsQueryModel : IRequest<DoctorsQueryResponse>
    {
        public List<string>? DoctorId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<DoctorSpecialism>? Specialisms { get; set; }
        public List<DoctorStatus>? Status { get; set; }
        public string? Name { get; set; }
        public string? SortDirection { get; set; }
        public string? SortBy { get; set; }

        public DoctorsQueryModel()
        {
        }
        public DoctorsQueryModel(DoctorsQuery query)
        {
            DoctorId = query.DoctorId == null ? null : new List<string>(query.DoctorId);
            Page = query.Page;
            PageSize = query.PageSize;
            Specialisms = query.Specialism == null ? null : query.Specialism.Select(x => ParseJobSpecialisms(x)).ToList();
            Status = query.Status == null ? null : query.Status.Select(x => ParseJobStatus(x)).ToList();
            Name = query.DoctorName == null ? null : query.DoctorName;
            SortDirection = query.SortDirection;
            SortBy = query.SortBy;
        }

        private DoctorStatus ParseJobStatus(string status)
        => Enum.TryParse<DoctorStatus>(status, true, out var result) ? result : DoctorStatus.Inactive;

        private DoctorSpecialism ParseJobSpecialisms(string specialisms)
        => Enum.TryParse<DoctorSpecialism>(specialisms, true, out var result) ? result : DoctorSpecialism.NotKnown;
    }
}
