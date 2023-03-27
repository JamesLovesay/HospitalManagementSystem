using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Commands.Patients
{
    public class DeletePatientCommand : IRequest<bool>
    {
        [Required]
        public string PatientId { get; set; }

        public DeletePatientCommand(string patientId)
        {
            PatientId = patientId;
        }
    }
}