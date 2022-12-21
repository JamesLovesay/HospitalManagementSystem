using FluentValidation;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Models;

namespace HospitalManagementSystem.Api.Validators
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorValidator()
        {
            When(x => x.Status != null, () =>
            {
                RuleFor(cmd => cmd)
                .Must(x => IsValidStatus(x.Status).valid)
                .WithMessage(x => GetInvalidStatusErrorMessage(x.Status));
            });

            When(x => x.Specialism != null, () =>
            {
                RuleFor(cmd => cmd)
                .Must(x => IsValidSpecialism(x.Specialism).valid)
                .WithMessage(x => GetInvalidSpecialismsErrorMessage(x.Specialism));
            });

            When(x => x.Name != null, () =>
            {
                RuleFor(cmd => cmd)
                .Must(x => x.Name.Length >= 0)
                .WithMessage(x => "Name cannot be empty");
            });

            RuleFor(cmd => cmd)
                .Must(x => x.HourlyChargingRate >= 0)
                .WithMessage(x => "Hourly charging Rate must be greater than 0");
        }

        private static (bool valid, string specialism) IsValidSpecialism(string specialism)
            => (Enum.TryParse<DoctorSpecialism>(specialism, true, out var result), specialism);

        private static (bool valid, string status) IsValidStatus(string status)
            => (Enum.TryParse<DoctorStatus>(status, true, out var result), status);

        private static string GetInvalidSpecialismsErrorMessage(string specialism)
            => $"Specialism value(s) supplied were invalid: {GetInvalidSpecialisms(specialism)}";

        private static string GetInvalidStatusErrorMessage(string status)
            => $"Status value(s) supplied were invalid: {GetInvalidStatus(status)}";

        private static string? GetInvalidSpecialisms(string specialism)
        {
            var valid = IsValidSpecialism(specialism);
            return !valid.valid ? valid.specialism : null;
        }

        private static string? GetInvalidStatus(string status)
        {
            var valid = IsValidStatus(status);
            return !valid.valid ? valid.status : null;
        }
    }
}
