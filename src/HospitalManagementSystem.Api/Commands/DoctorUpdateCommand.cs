using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Commands
{
    public class DoctorUpdateCommand : IRequest<bool>
    {
    [Required]
    public string DoctorId { get; set; }
    public string? Name { get; set; }
    public decimal? HourlyChargingRate { get; set; }
    public string? Status { get; set; }
    }
}