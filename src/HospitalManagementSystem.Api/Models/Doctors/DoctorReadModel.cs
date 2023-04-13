using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Models.Doctors
{
    public class DoctorReadModel
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public decimal HourlyChargingRate { get; set; }
        public string Status { get; set; }
        public string Specialism { get; set; }
    }
}
