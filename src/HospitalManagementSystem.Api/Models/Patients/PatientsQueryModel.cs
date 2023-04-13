using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Queries.Patients;
using MediatR;

namespace HospitalManagementSystem.Api.Models.Patients
{
    public class PatientsQueryModel : IRequest<PatientsQueryResponse>
    {
        public List<string?> PatientId { get; set; } = new List<string?>();
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? Status { get; set; }
        public string? Name { get; set; }
        public string? SortDirection { get; set; }
        public string? SortBy { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public string? AdmissionDate { get; set; }
        public bool isAdmitted { get; set; }

        public PatientsQueryModel()
        {
        }

        public PatientsQueryModel(PatientsQuery query)
        {
            PatientId = query.PatientId == null ? new List<string?>() : new List<string?>(query.PatientId);
            Page = query.Page;
            PageSize = query.PageSize;
            SortDirection = query.SortDirection;
            SortBy = query.SortBy;
            Gender = query.Gender == null ? null : ParseGender(query.Gender).ToString();
            DateOfBirth = query.DateOfBirth;
            AdmissionDate = query.AdmissionDate;
            isAdmitted = query.isAdmitted;
            Name = query.PatientName == null ? null : query.PatientName;
            Status = query.Status == null ? null : ParsePatientStatus(query.Status).ToString();
        }

        private GenderValue ParseGender(string gender)
        => Enum.TryParse<GenderValue>(gender, true, out var result) ? result : GenderValue.Unknown;

        private PatientStatus ParsePatientStatus(string status)
        => Enum.TryParse<PatientStatus>(status, true, out var result) ? result : PatientStatus.Unknown;
    }
}