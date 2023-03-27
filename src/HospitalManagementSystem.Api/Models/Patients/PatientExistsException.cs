using System.Runtime.Serialization;

namespace HospitalManagementSystem.Api.Models.Patients
{
    [Serializable]
    internal class PatientExistsException : Exception
    {
        public PatientExistsException()
        {
        }

        public PatientExistsException(string? message) : base(message)
        {
        }

        public PatientExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PatientExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}