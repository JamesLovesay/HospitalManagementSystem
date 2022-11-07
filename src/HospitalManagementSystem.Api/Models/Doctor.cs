namespace HospitalManagementSystem.Api.Models
{
    public class Doctor
    {
        public Guid DoctorId { get; set; }
        public string Name { get; set; }
        public decimal HourlyChargingRate { get; set; }
        public DoctorStatus Status { get; set; }
        public DoctorSpecialism Specialism { get; set; }

        public Doctor(string name, decimal hourlyChargingRate, DoctorSpecialism doctorSpecialism, DoctorStatus status)
        {
            DoctorId = Guid.NewGuid();
            Name = name;
            HourlyChargingRate = hourlyChargingRate;
            Specialism = doctorSpecialism;
            Status = status;
        }
    }
}
