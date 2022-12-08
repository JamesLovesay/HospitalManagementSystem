using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Validators
{
    public class DoctorSortByValidator : ValidationAttribute
    {
        public string GetErrorMessage() => "Sort By must either be Name, Status, Specialism or HourlyChargingRate";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var doctor = (DoctorsQuery)validationContext.ObjectInstance;
            var validSortBy = new List<string> { nameof(Doctor.Name), nameof(Doctor.Specialism), nameof(Doctor.Status), nameof(Doctor.HourlyChargingRate) };

            if (doctor.SortBy == null) return ValidationResult.Success;
            if (validSortBy.Contains(doctor.SortBy))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }
    }
}
