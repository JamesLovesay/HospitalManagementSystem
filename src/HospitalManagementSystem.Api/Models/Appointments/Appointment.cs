using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Models.Appointments;

public class Appointment
{
    [Required]
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Description { get; set; }
    public int? DoctorId { get; set; }
    public int? PatientId { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }

    public Appointment() { }

    public Appointment(Guid id, string? description, int? patientId, string startTime, string endTime, string doctorName, string patientName, int? doctorId)
    {
        Id = id;
        Description = description;
        StartTime = DateTime.TryParse(startTime, out DateTime startResult) ? startResult : DateTime.MinValue;
        EndTime = DateTime.TryParse(endTime, out DateTime endResult) ? endResult : DateTime.MinValue;
        PatientId = patientId;
        PatientName = patientName;
        DoctorName = doctorName;
        DoctorId = doctorId;
    }
}
