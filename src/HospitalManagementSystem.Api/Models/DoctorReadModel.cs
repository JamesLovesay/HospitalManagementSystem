using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Models
{
    public class DoctorReadModel
    {
        public ObjectId _id { get; set; }
        public ObjectId DoctorId { get; set; }
        public string Name { get; set; }
        public decimal HourlyChargingRate { get; set; }
        public DoctorStatus Status { get; set; }
        public DoctorSpecialism Specialism { get; set; }
    }
}
