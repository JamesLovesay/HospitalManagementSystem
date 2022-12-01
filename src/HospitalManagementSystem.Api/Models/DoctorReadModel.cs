using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Models
{
    public class DoctorReadModel
    {
        public ObjectId _id { get; set; }
        public ObjectId DoctorId { get; set; }
        public string Name { get; set; }
        public decimal HourlyChargingRate { get; set; }
        public string? Status { get; set; }
        public string? Specialism { get; set; }
    }
}
