using FluentValidation;
using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Patients;

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
        }

        private static (bool valid, string status) IsValidStatus(string status)
            => (Enum.TryParse<PatientStatus>(status, true, out var result), status);

        private static string GetInvalidStatusErrorMessage(List<string> statusList)
            => $"Status value(s) supplied were invalid: {GetInvalidStatus(statusList)}";

        private static string GetInvalidStatus(List<string> statusList)
            => string.Join(", ", statusList.Select(x => IsValidStatus(x))
            .Where(x => !x.valid)
            .Select(x => x.status));
    }
}