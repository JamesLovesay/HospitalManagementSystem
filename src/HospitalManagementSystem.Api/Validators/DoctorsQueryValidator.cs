using FluentValidation;
using HospitalManagementSystem.Api.Helpers;
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
        }
    }
}
