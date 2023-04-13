using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HospitalManagementSystem.Api.Models.Doctors
{
    public class Doctor
    {
        [BsonId]
        public string DoctorId { get; set; }
        public string Name { get; set; }
        public decimal HourlyChargingRate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public DoctorStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public DoctorSpecialism Specialism { get; set; }

        public static Doctor From(DoctorReadModel model)
        {
            return new Doctor
            {
                DoctorId = model._id,
                Name = model.Name,
                HourlyChargingRate = model.HourlyChargingRate,
                Specialism = ParseJobSpecialisms(model.Specialism),
                Status = ParseJobStatus(model.Status)
            };
        }

        private static DoctorStatus ParseJobStatus(string status)
        => Enum.TryParse<DoctorStatus>(status, true, out var result) ? result : DoctorStatus.Inactive;

        private static DoctorSpecialism ParseJobSpecialisms(string specialisms)
        => Enum.TryParse<DoctorSpecialism>(specialisms, true, out var result) ? result : DoctorSpecialism.NotKnown;
    }
}
