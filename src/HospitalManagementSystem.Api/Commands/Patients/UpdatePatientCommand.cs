using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Commands.Patients
{
    public class UpdatePatientCommand : IRequest<bool>
    {
        [Required]
        public string? PatientId { get; set; }
        public string? Name { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AdmissionDate { get; set; }
        public bool? IsAdmitted { get; set; }
        public string? Status { get; set; }
        public int? RoomId { get; set; }
        public string? Gender { get; set; }
    }
}