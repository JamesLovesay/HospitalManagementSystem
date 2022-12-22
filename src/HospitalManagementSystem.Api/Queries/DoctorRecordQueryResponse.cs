using HospitalManagementSystem.Api.Models;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorRecordQueryResponse
    {
        private readonly bool _notFoundInReadStore;

        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialism {  get; set; }
        public string Status { get; set; }
        public decimal HourlyChargingRate { get; set; }


        public DoctorRecordQueryResponse() { }

        public DoctorRecordQueryResponse(bool notFoundInReadStore) : this()
        {
            _notFoundInReadStore = notFoundInReadStore;
        }

        public bool NotFoundInReadStore() => _notFoundInReadStore;

        public static DoctorRecordQueryResponse Empty()
            => new DoctorRecordQueryResponse(true);

        public static DoctorRecordQueryResponse From(DoctorReadModel model)
            => new DoctorRecordQueryResponse
            {
                DoctorId = model._id,
                DoctorName = model.Name,
                Specialism = model.Specialism,
                Status = model.Status,
                HourlyChargingRate = model.HourlyChargingRate,
            };

        public bool IsReady()
        {
            return !IsEmpty();
        }

        private bool IsEmpty() => string.IsNullOrWhiteSpace(DoctorId);
    }
}