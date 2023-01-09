using FluentValidation;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Models;

namespace HospitalManagementSystem.Api.Validators
{
    public class UpdateDoctorCommandValidator : AbstractValidator<DoctorUpdateCommand>
    {
        public UpdateDoctorCommandValidator()
        {
            When(x => x.Status != null, () =>
            {
                RuleFor(cmd => cmd)
                .Must(x => IsValidStatus(x.Status).valid)
                .WithMessage(x => GetInvalidStatusErrorMessage(x.Status));
            });

            RuleFor(cmd => cmd)
                .Must(x => x.HourlyChargingRate >= 0)
                .WithMessage(x => "Hourly charging Rate must be greater than 0");
        }

        private static (bool valid, string status) IsValidStatus(string status)
            => (Enum.TryParse<DoctorStatus>(status, true, out var result), status);

        private static string GetInvalidStatusErrorMessage(string status)
            => $"Status value(s) supplied were invalid: {GetInvalidStatus(status)}";

        private static string? GetInvalidStatus(string status)
        {
            var valid = IsValidStatus(status);
            return !valid.valid ? valid.status : null;
        }
    }
}
