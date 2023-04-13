using HospitalManagementSystem.Api.Models.Patients;
using System.Net;

namespace HospitalManagementSystem.Api.Queries.Patients
{
    public class PatientRecordQueryResponse
    {
        private readonly bool _notFoundInReadStore;

        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string AdmissionDate { get; set; }
        public bool IsAdmitted { get; set; }
        public string PatientStatus { get; set; }
        public int? RoomId { get; set; }

        public PatientRecordQueryResponse() { }

        public PatientRecordQueryResponse(bool notFoundInReadStore) : this()
        {
            _notFoundInReadStore = notFoundInReadStore;
        }

        public bool NotFoundInReadStore() => _notFoundInReadStore;

        public static PatientRecordQueryResponse Empty()
            => new PatientRecordQueryResponse(true);

        public static PatientRecordQueryResponse From(PatientReadModel model)
            => new PatientRecordQueryResponse
            {
                PatientId = model._id,
                PatientName = model.Name,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                EmailAddress = model.Email,
                PhoneNumber = model.PhoneNumber,
                AdmissionDate = model.AdmissionDate,
                IsAdmitted = model.IsAdmitted,
                PatientStatus = model.PatientStatus,
                RoomId = model.RoomId,
            };

        public bool IsReady()
        {
            return !IsEmpty();
        }

        private bool IsEmpty() => string.IsNullOrWhiteSpace(PatientId);
    }
}