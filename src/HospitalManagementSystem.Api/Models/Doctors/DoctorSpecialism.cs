namespace HospitalManagementSystem.Api.Models.Doctors
{
    public enum DoctorSpecialism
    {
        // Must be kept in alphabetical order aside from Not Known to protect sort function in repository
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