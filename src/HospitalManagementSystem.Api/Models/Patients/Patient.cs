using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Models.Patients
{
    public class Patient
    {
        public string? PatientId { get; set; }
        [Required]
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime AdmissionDate { get; set; }
        public bool IsAdmitted { get; set; }
        public PatientStatus Status { get; set; }
        public int? RoomId { get; set; }
        public GenderValue Gender { get; set; }

        public static Patient From(PatientReadModel model)
        {
            return new Patient
            {
                PatientId = model._id,
                Name = model.Name,
                IsAdmitted = model.IsAdmitted,
                RoomId = model.RoomId,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AdmissionDate = DateTime.Parse(model.AdmissionDate),
                DateOfBirth = DateTime.Parse(model.DateOfBirth),
                Gender = ParseGenderValue(model.Gender),
                Status = ParsePatientStatus(model.PatientStatus)
            };
        }

        private static PatientStatus ParsePatientStatus(string status)
        => Enum.TryParse<PatientStatus>(status, true, out var result) ? result : PatientStatus.Unknown;

        private static GenderValue ParseGenderValue(string gender)
        => Enum.TryParse<GenderValue>(gender, true, out var result) ? result : GenderValue.Unknown;

        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth > today.AddYears(-age)) age--;

            return age;
        }

        public bool IsEligibleForDischarge()
        {
            // A patient must be admitted and have stayed for at least one day to be eligible for discharge
            return IsAdmitted && AdmissionDate < DateTime.Today.AddDays(-1);
        }
    }

    public enum GenderValue
    {
        Unknown,
        Female,
        Male,
        NonBinary
    }

    public enum PatientStatus
    {
        Unknown,
        Discharged,
        InTreatment,
        Registered,
    }
}
