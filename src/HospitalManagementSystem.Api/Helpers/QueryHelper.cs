using HospitalManagementSystem.Api.Models;

namespace HospitalManagementSystem.Api.Helpers
{
    public class QueryHelper
    {
        public static int MaximumPageSize = 100;
        public static string SortAscending = "ASC";
        public static string SortDescending = "DESC";
        public static List<string> DoctorSortableFields = new List<string>() 
        { 
            nameof(Doctor.Specialism),
            nameof(Doctor.Status),
            nameof(Doctor.Name),
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
    }
}
