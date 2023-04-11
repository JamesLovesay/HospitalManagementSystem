using FluentValidation;
using HospitalManagementSystem.Api.Commands.Patients;

namespace HospitalManagementSystem.Api.Validators.Patients;

public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("The FirstName field is required.")
            .MaximumLength(50).WithMessage("First Name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("The LastName field is required.")
            .MaximumLength(50).WithMessage("Last Name must not exceed 50 characters.");

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

        When(x => x.DateOfBirth != null, () =>
        {
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(BeValidDate).WithMessage("Invalid date of birth.")
                .Must(DateNotInFuture).WithMessage("Date of birth must be in the past.");
        });

        When(x => x.AdmissionDate != null, () =>
        {
            RuleFor(x => x.AdmissionDate)
                .Must(BeValidDate).WithMessage("Invalid date of admission.")
                .Must(DateNotInFuture).WithMessage("Admission date cannot be in the future.");
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

    private bool BeValidDate(string? dobString)
    {
        DateTime dob;
        return DateTime.TryParse(dobString, out dob);
    }

    private bool DateNotInFuture(string? dobString)
    {
        DateTime dob;
        if (DateTime.TryParse(dobString, out dob))
        {
            return dob <= DateTime.UtcNow;
        }
        return false;
    }
}