using HospitalManagementSystem.Api.Models.Appointments;

namespace HospitalManagementSystem.Api.Models.Patients
{
    public class PatientReadModel
    {
        public string? _id { get; set; }
        public string? Name { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? PatientStatus { get; set; }
        public bool IsAdmitted { get; set; }
        public string? AdmissionDate { get; set; }
        public int? RoomId { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}