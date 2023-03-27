using MediatR;

namespace HospitalManagementSystem.Api.Queries.Patients
{
    public class PatientsQuery : IRequest<PatientsQueryResponse>
    {
        public List<string>? PatientId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public string? PatientName { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public string? AdmissionDate { get; set; }
        public bool isAdmitted { get; set; }
    }
}