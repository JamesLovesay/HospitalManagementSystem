using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Queries;
using MediatR;
using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Models
{
    public class DoctorsQueryModel : IRequest<DoctorsQueryResponse>
    {
        public List<ObjectId>? DoctorIds { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<DoctorSpecialism>? Specialisms { get; set; }
        public List<DoctorStatus>? Status { get; set; }
        public string? Name { get; set; }
        public string SortDirection { get; set; }
        public string SortBy { get; set; }


        public DoctorsQueryModel()
        {

        }
        public DoctorsQueryModel(DoctorsQuery query)
        {
            DoctorIds = query.DoctorIds == null ? null : new List<ObjectId>(query.DoctorIds);
            Page = query.Page;
            PageSize = query.PageSize;
            Specialisms = GetSpecialisms(query.Specialisms);
            Status = GetStatuses(query.Statuses);
            Name = query.DoctorName == null ? null : query.DoctorName;
            SortDirection = query.SortDirection;
            SortBy = query.SortBy;
        }

        private List<DoctorStatus> GetStatuses(List<string> statuses)
        => statuses == null ? null : statuses.Select(x => ParseJobStatus(x)).ToList();

        private DoctorStatus ParseJobStatus(string status)
        => Enum.TryParse<DoctorStatus>(status, true, out var result) ? result : DoctorStatus.Inactive;

        private List<DoctorSpecialism> GetSpecialisms(List<string> specialisms)
        => specialisms == null ? null : specialisms.Select(x => ParseJobSpecialisms(x)).ToList();

        private DoctorSpecialism ParseJobSpecialisms(string specialisms)
        => Enum.TryParse<DoctorSpecialism>(specialisms, true, out var result) ? result : DoctorSpecialism.NotKnown;
    }
}
