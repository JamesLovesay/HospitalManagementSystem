using FluentAssertions;
using HospitalManagementSystem.Api.Events.Appointments;
using HospitalManagementSystem.Api.Models.Appointments;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Events.Appointments;

public class AppointmentCreatedEventHandlerTests
{
    private readonly Mock<IPatientsRepository> _repository;
    private readonly Mock<ILogger<AppointmentCreatedEventHandler>> _logger;
    private readonly AppointmentCreatedEventHandler _handler;

    public AppointmentCreatedEventHandlerTests()
    {
        _repository = new Mock<IPatientsRepository>();
        _logger = new Mock<ILogger<AppointmentCreatedEventHandler>>();
        _handler = new AppointmentCreatedEventHandler(_logger.Object, _repository.Object);
    }

    [Fact]
    public async Task Handle_WithValidNotification_ShouldUpdatePatientAndLogInformation()
    {
        // Arrange
        var appointment = new Appointment(Guid.NewGuid(), "Test Appointment", 1, "2023-04-12T14:30:00", "2023-04-12T1%:30:00", "Doctor Name", "Patient Name", 2);
        var notification = new AppointmentCreatedEvent(appointment);
        var patient = new PatientReadModel { _id = appointment.PatientId.ToString(), Appointments = new List<Appointment> { appointment } };
        _repository.Setup(x => x.GetPatientById(It.IsAny<string>())).ReturnsAsync(patient);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _repository.Verify(x => x.UpsertPatient(It.IsAny<PatientReadModel>()), Times.Once);
        patient.Appointments.Should().Contain(appointment);
    }

    [Fact]
    public async Task Handle_WithInvalidNotification_ShouldNotUpdatePatientAndLogInformation()
    {
        // Arrange
        var notification = new AppointmentCreatedEvent(null!);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _repository.Verify(x => x.UpsertPatient(It.IsAny<PatientReadModel>()), Times.Never);
    }
}
