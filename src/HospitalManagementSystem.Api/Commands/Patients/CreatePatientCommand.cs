using HospitalManagementSystem.Api.Models.Patients;
using MediatR;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Commands.Patients
{
    public class CreatePatientCommand : IRequest<ObjectId?>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AdmissionDate { get; set; }
        public bool IsAdmitted { get; set; }
        public string? Status { get; set; }
        public int? RoomId { get; set; }
        public string? Gender { get; set; }
    }
}