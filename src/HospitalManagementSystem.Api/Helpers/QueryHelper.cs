using HospitalManagementSystem.Api.Models.Doctors;
using HospitalManagementSystem.Api.Models.Patients;
using MongoDB.Driver;

namespace HospitalManagementSystem.Api.Helpers
{
    public class QueryHelper
    {
        public static int MaximumPageSize = 100;
        public static string SortAscending = "ASC";
        public static string SortDescending = "DESC";
        public static int DefaultPageSize = 20;
        public static List<string> DoctorSortableFields = new List<string>() 
        {
            nameof(Doctor.Name),
            nameof(Doctor.Specialism),
            nameof(Doctor.Status),
            nameof(Doctor.HourlyChargingRate)
        };
        public static List<string> DoctorSortableStatuses = new List<string>()
        {
            nameof(DoctorStatus.Inactive),
            nameof(DoctorStatus.ActiveVisiting),
            nameof(DoctorStatus.ActivePermanent)
        };
        public static List<string> DoctorSortableSpecialisms = new List<string>()
        {
            nameof(DoctorSpecialism.GeneralPractice),
            nameof(DoctorSpecialism.Psychiatry),
            nameof(DoctorSpecialism.Psychology),
            nameof(DoctorSpecialism.Gynaecology),
            nameof(DoctorSpecialism.Neurology),
            nameof(DoctorSpecialism.GeneralSurgery),
            nameof(DoctorSpecialism.Orthopaedics),
            nameof(DoctorSpecialism.Urology),
        };

        public static List<string> PatientSortableFields { get; internal set; } = new List<string>()
        {
            nameof(Patient.Name),
            nameof(Patient.AdmissionDate),
            nameof(Patient.DateOfBirth)
        };
        public static List<string> PatientGenderValues { get; internal set; } = new List<string>()
        {
            nameof(GenderValue.Male),
            nameof(GenderValue.Female),
            nameof(GenderValue.NonBinary)
        };
        public const string DateTimeFormat = "yyyy-MM-dd";

        public static (SortDefinition<DoctorReadModel> sortDefinition, string option, string direction) GetDoctorSortDetails(string sortBy, string sortDirection)
        {
            var option = (new[] { nameof(Doctor.Name), nameof(Doctor.Specialism), nameof(Doctor.Status), nameof(Doctor.HourlyChargingRate) }
                            .FirstOrDefault(x => x.Equals(sortBy, StringComparison.InvariantCultureIgnoreCase)))
                            ?? nameof(Doctor.Name);

            var direction = (sortDirection ?? SortDescending).ToUpperInvariant();

            var sort = direction == SortAscending
                        ? Builders<DoctorReadModel>.Sort.Ascending(option)
                        : Builders<DoctorReadModel>.Sort.Descending(option);
            return (sort, option, direction);
        }

        public static (SortDefinition<PatientReadModel> sortDefinition, string option, string direction) GetPatientSortDetails(string sortBy, string sortDirection)
        {
            var option = (new[] { nameof(Patient.Name), nameof(Patient.DateOfBirth), nameof(Patient.AdmissionDate), nameof(Patient.Status) }
                            .FirstOrDefault(x => x.Equals(sortBy, StringComparison.InvariantCultureIgnoreCase)))
                            ?? nameof(Doctor.Name);

            var direction = (sortDirection ?? SortDescending).ToUpperInvariant();

            var sort = direction == SortAscending
                        ? Builders<PatientReadModel>.Sort.Ascending(option)
                        : Builders<PatientReadModel>.Sort.Descending(option);
            return (sort, option, direction);
        }
    }
}
