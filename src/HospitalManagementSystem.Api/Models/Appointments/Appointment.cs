using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Models.Appointments;

public class Appointment
{
    [Required]
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Description { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }
}
