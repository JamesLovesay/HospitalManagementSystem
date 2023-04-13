using MediatR;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Commands.Doctors
{
    public class CreateDoctorCommand : IRequest<ObjectId?>
    {
        [Required]
        public string Name { get; set; }
        public int HourlyChargingRate { get; set; }
        public string? Status { get; set; }
        public string? Specialism { get; set; }
    }
}