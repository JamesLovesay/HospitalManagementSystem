using HospitalManagementSystem.Api.Models;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorRecordQueryResponse
    {
        private readonly bool _notFoundInReadStore;

        public string DoctorId { get; set; }


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
            };

        public bool IsReady()
        {
            return !IsEmpty();
        }

        private bool IsEmpty() => string.IsNullOrWhiteSpace(DoctorId);
    }
}