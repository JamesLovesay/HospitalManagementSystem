using FluentValidation;
using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Patients;
using System.Globalization;

namespace HospitalManagementSystem.Api.Validators.Patients
{
    public class PatientsQueryValidator : AbstractValidator<PatientsQuery>
    {
        public PatientsQueryValidator()
        {
            When(x => x.Page != null, () =>
            {
                RuleFor(cmd => cmd.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
            });

            When(x => x.PageSize != null, () =>
            {
                RuleFor(cmd => cmd.PageSize).GreaterThan(0).WithMessage("Page Size must be greater than 0");
                RuleFor(cmd => cmd.PageSize).LessThanOrEqualTo(QueryHelper.MaximumPageSize).WithMessage($"Page Size must be less than {QueryHelper.MaximumPageSize}");
            });

            When(x => x.SortBy != null, () =>
            {
                RuleFor(cmd => cmd.SortBy).Must(x => QueryHelper.PatientSortableFields.Any(y => y.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                    .WithMessage("You can only sort on fields FirstName, LastName, DateOfBirth or AdmissionDate");
            });

            When(x => x.SortDirection != null, () =>
            {
                RuleFor(cmd => cmd.SortDirection).Must(x => x.Equals(QueryHelper.SortDescending, StringComparison.InvariantCultureIgnoreCase) ||
                                                            x.Equals(QueryHelper.SortAscending, StringComparison.InvariantCultureIgnoreCase))
                .WithMessage("You can only sort by directions asc or desc");

            });

            When(x => x.PatientName != null, () =>
            {
                RuleFor(cmd => cmd.PatientName).Matches("^[a-zA-Z\\s]*$").WithMessage("Invalid PatientName, only alphabetical characters allowed.");
            });

            When(x => x.Gender != null, () =>
            {
                RuleFor(cmd => cmd.Gender).Must(x => QueryHelper.PatientGenderValues.Any(y => y.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                .WithMessage("Gender can only be Male, Female, or NonBinary");
            });

            When(x => x.DateOfBirth != null, () =>
            {
                RuleFor(cmd => cmd.DateOfBirth)
                    .Must(dateString => DateTime.TryParseExact(dateString, QueryHelper.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    .WithMessage($"DateOfBirth must match the format {QueryHelper.DateTimeFormat}");

            });

            When(x => x.AdmissionDate != null, () =>
            {
                RuleFor(cmd => cmd.DateOfBirth)
                    .Must(dateString => DateTime.TryParseExact(dateString, QueryHelper.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    .WithMessage($"DateOfBirth must match the format {QueryHelper.DateTimeFormat}");

            });

            When(x => x.Status != null, () =>
            {
                RuleFor(cmd => cmd)
                .Must(x => IsValidStatus(x.Status).valid)
                .WithMessage($"Status value supplied was invalid. Can only be Discharged, Admitted or InTreatment");
            });
        }

        private static (bool valid, string status) IsValidStatus(string status)
            => (Enum.TryParse<PatientStatus>(status, true, out var result), status);
    }
}