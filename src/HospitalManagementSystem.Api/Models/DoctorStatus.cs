namespace HospitalManagementSystem.Api.Models
{
    public enum DoctorStatus
    {
        // Must be kept in laphabetical order to protect sort function in repository
        ActivePermanent,
        ActiveVisiting,
        Inactive
    }
}
