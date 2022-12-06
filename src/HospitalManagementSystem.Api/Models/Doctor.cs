using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace HospitalManagementSystem.Api.Models
{
    public class Doctor
    {
        [BsonId]
        public ObjectId DoctorId { get; set; }
        public string Name { get; set; }
        public decimal HourlyChargingRate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DoctorStatus Status { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DoctorSpecialism Specialism { get; set; }

        public static Doctor From (DoctorReadModel model)
        {
            return new Doctor
            {
                DoctorId = model.DoctorId,
                Name = model.Name,
                HourlyChargingRate= model.HourlyChargingRate,
                Specialism = model.Specialism,
                Status = model.Status
            };
        }
    
        //public Doctor(string name, decimal hourlyChargingRate, DoctorSpecialism doctorSpecialism, DoctorStatus status)
        //{
        //    DoctorId = Guid.NewGuid();
        //    Name = name;
        //    HourlyChargingRate = hourlyChargingRate;
        //    Specialism = doctorSpecialism;
        //    Status = status;
        //}
    }
}
