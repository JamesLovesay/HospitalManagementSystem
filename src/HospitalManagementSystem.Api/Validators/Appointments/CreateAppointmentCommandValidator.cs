using FluentValidation;
using HospitalManagementSystem.Api.Commands.Appointments;

namespace HospitalManagementSystem.Api.Validators.Appointments;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("The PatientId field is required.")
            .GreaterThan(0).WithMessage("The PatientId field must be greater than 0.");

        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("The DoctorId field is required.")
            .GreaterThan(0).WithMessage("The DoctorId field must be greater than 0.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("The StartTime field is required.")
            .Must(BeValidDateTime).WithMessage("The StartTime field must be a valid date/time string in the format yyyy-MM-ddTHH:mm:ss.");

        When(x => !string.IsNullOrWhiteSpace(x.EndTime), () =>
        {
        RuleFor(x => x.EndTime)
            .Must(BeValidDateTime).When(x => !string.IsNullOrEmpty(x.EndTime)).WithMessage("The EndTime field must be a valid date/time string in the format yyyy-MM-ddTHH:mm:ss.");
        });
     }

    private bool BeValidDateTime(string value)
    {
        return DateTime.TryParse(value, out _);
    }
}
