namespace HospitalManagementSystem.Api.Models
{
    public enum DoctorSpecialism
    {
        // Must be kept in laphabetical order aside from Not Known to protect sort function in repository
        NotKnown,
        GeneralPractice,
        GeneralSurgery,
        Gynaecology,
        Neurology,
        Orthopaedics,
        Psychiatry,
        Psychology,
        Urology
    }
}