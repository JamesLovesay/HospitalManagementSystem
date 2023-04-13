using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Commands.Doctors
{
    public class DoctorDeleteCommand : IRequest<bool>
    {
        [Required]
        public string DoctorId { get; set; }
    }
}