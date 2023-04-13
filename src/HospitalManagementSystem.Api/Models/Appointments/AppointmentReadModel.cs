namespace HospitalManagementSystem.Api.Models.Appointments;

public class AppointmentReadModel
{
    public int Id { get; set; }
    public string StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? Description { get; set; }
    public string? DoctorName { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorId { get; set;}
    public string? PatientId { get; set;}

    public AppointmentReadModel(int id, string startTime)
    {
        Id = id;
        StartTime = startTime;
    }
}
