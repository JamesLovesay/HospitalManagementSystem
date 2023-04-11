using DnsClient;
using FluentValidation;
using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Models.Patients;

namespace HospitalManagementSystem.Api.Validators.Patients
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator()
        {
            RuleFor(cmd => cmd.PatientId).NotEmpty().WithMessage("Patient ID is required.");

            When(x => x.Gender != null, () =>
            {
                RuleFor(cmd => cmd.Gender)
                    .Must(BeValidGender).WithMessage("Invalid gender value.");
            });

            When(x => x.DateOfBirth != null, () =>
            {
                RuleFor(cmd => cmd.DateOfBirth)
                    .Must(BeValidDate).WithMessage("Invalid date of birth.")
                    .Must(DateNotInFuture).WithMessage("Date of birth must be in the past.");
            });

            When(x => x.AdmissionDate != null, () =>
            {
                RuleFor(cmd => cmd.AdmissionDate)
                    .Must(BeValidDate).WithMessage("Invalid date of admission.")
                    .Must(DateNotInFuture).WithMessage("Admission date cannot be in the future.");
            });

            When(x => x.PhoneNumber != null, () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .Matches(@"^\+?[0-9\s]*$").WithMessage("Invalid phone number format.");
            });

            When(x => x.Email != null, () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("Invalid email address format.");
            });

            When(x => x.RoomId != null, () =>
            {
                RuleFor(x => x.RoomId)
                    .GreaterThan(0).When(x => x.RoomId.HasValue).WithMessage("Room ID must be greater than 0.");
            });

            When(x => x.Status != null, () =>
            {
                RuleFor(x => x.Status)
                    .IsInEnum().WithMessage("Invalid patient status.");
            });
        }

        private bool BeValidGender(string? gender)
            => Enum.TryParse<GenderValue>(gender, true, out var result);

        private bool BeValidDate(string? dateString)
        {
            DateTime dob;
            return DateTime.TryParse(dateString, out dob);
        }

        private bool DateNotInFuture(string? dateString)
        {
            DateTime date;
            DateTime.TryParse(dateString, out date);
            return  date <= DateTime.Today;
        }

    }
}