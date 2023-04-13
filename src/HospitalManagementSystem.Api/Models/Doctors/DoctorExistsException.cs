namespace HospitalManagementSystem.Api.Models.Doctors
{
    public class DoctorExistsException : Exception
    {
        public DoctorExistsException() : base("This is a custom exception.") { }
        public DoctorExistsException(string message) : base(message) { }
    }
}
