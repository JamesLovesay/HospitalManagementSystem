using HospitalManagementSystem.Api.Repositories.Interfaces;

namespace HospitalManagementSystem.Api.Events.Appointments;

public class AppointmentCreatedEventHandler : IEventHandler<AppointmentCreatedEvent>
{
    private readonly ILogger<AppointmentCreatedEventHandler> _logger;
    private readonly IPatientsRepository _patientRepository;

    public AppointmentCreatedEventHandler(ILogger<AppointmentCreatedEventHandler> logger, IPatientsRepository repository)
    {
        _logger = logger;
        _patientRepository = repository;
    }

    public async Task Handle(AppointmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Retrieve the patient by ID
        var patient = await _patientRepository.GetPatientById(notification.Appointment?.PatientId?.ToString());

        if (patient != null)
        {
            // Add the new appointment to the patient's list of appointments
            patient.Appointments.Add(notification.Appointment);

            // Update the patient in the repository
            await _patientRepository.UpsertPatient(patient);
        }
        
        //Consider additional implementation in due course - eg notify doctor - add to caledar etc

        _logger.LogInformation($"Appointment with ID {notification.Appointment.Id} created.");
    }
}
