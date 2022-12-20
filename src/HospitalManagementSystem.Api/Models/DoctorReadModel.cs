using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Models
{
    public class DoctorReadModel
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public decimal HourlyChargingRate { get; set; }
        public DoctorStatus Status { get; set; }
        public DoctorSpecialism Specialism { get; set; }
    }
}
