using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries.Doctors;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Validators.Doctors
{
    public class DoctorSortDirectionValidator : ValidationAttribute
    {
        public string GetErrorMessage() => "Sort Direction must either be asc or desc";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var doctor = (DoctorsQuery)validationContext.ObjectInstance;
            var validSortDirection = new List<string> { QueryHelper.SortDescending, QueryHelper.SortAscending };

            if (doctor.SortDirection == null) return ValidationResult.Success;
            if (validSortDirection.Contains(doctor.SortDirection))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }
    }
}
