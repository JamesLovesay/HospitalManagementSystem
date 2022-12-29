using MediatR;

namespace HospitalManagementSystem.Api.Commands
{
    public class DoctorUpdateCommand : IRequest<bool>
    {
    public string DoctorId { get; set; }
    public string? Name { get; set; }
    public decimal? HourlyChargingRate { get; set; }
    public string? Status { get; set; }
    }
}