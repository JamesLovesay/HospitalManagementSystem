using FluentValidation;
using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace HospitalManagementSystem.Api.Validators
{
    public class DoctorsQueryValidator : AbstractValidator<DoctorsQuery>
    {
        public DoctorsQueryValidator()
        {
            When(x => x.Page != null, () =>
            {
                RuleFor(cmd => cmd.Page).GreaterThan(0);
            });

            When(x => x.PageSize != null, () =>
            {
                RuleFor(cmd => cmd.PageSize).GreaterThan(0);
                RuleFor(cmd => cmd.PageSize).LessThanOrEqualTo(QueryHelper.MaximumPageSize);
            });

            When(x => x.SortBy != null, () =>
            {
                RuleFor(cmd => cmd.SortBy).Must(x => QueryHelper.DoctorSortableFields.Any(y => y.Equals(x, StringComparison.InvariantCultureIgnoreCase)));
            });

            When(x => x.SortDirection != null, () =>
            {
                RuleFor(cmd => cmd.SortDirection).Must(x => x.Equals(QueryHelper.SortDescending, StringComparison.InvariantCultureIgnoreCase) ||
                                                            x.Equals(QueryHelper.SortAscending, StringComparison.InvariantCultureIgnoreCase));
            });

            When(x => x.Statuses != null, () =>
            {
                RuleFor(cmd => cmd.Statuses).Must(x => QueryHelper.DoctorSortableStatuses.Any(y => y.Equals(x)));
            });

            When(x => x.Specialisms != null, () =>
            {
                RuleFor(cmd => cmd)
                    .Must(x => x.Specialisms.All(y => IsValidSpecialism(y).valid))
                    .WithMessage(x => GetInvalidSpecialismsErrorMessage(x.Specialisms));
            });
        }

        private static (bool valid, string specialism) IsValidSpecialism(string specialism)
        => (Enum.TryParse<DoctorSpecialism>(specialism, true, out var result), specialism);

        private static string GetInvalidSpecialismsErrorMessage(List<string> specialisms)
        => $"Specialism value(s) supplied were invalid: {GetInvalidSpecialisms(specialisms)}";

        private static string GetInvalidSpecialisms(List<string> specialisms)
            => string.Join(", ", specialisms.Select(x => IsValidSpecialism(x))
            .Where(x => !x.valid)
            .Select(x => x.specialism));
    }
}
