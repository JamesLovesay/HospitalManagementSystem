using HospitalManagementSystem.Api.Models;
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
    }
}
