using FluentAssertions;
using HospitalManagementSystem.Api.Models.Appointments;
using HospitalManagementSystem.Api.Queries.Appointments;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries.Appointments;

public class AppointmentRecordQueryHandlerTests
{
    private readonly Mock<IAppointmentRepository> _mockRepo;
    private readonly AppointmentRecordQueryHandler _handler;

    public AppointmentRecordQueryHandlerTests()
    {
        _mockRepo = new Mock<IAppointmentRepository>();
        _handler = new AppointmentRecordQueryHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ExistingAppointment_ReturnsAppointmentRecordQueryResponse()
    {
        try
        {
            // Arrange
            var appointmentId = 1234;
            var appointmentReadModel = new AppointmentReadModel(appointmentId, "2023-04-12T16:30:00")
            {
                PatientId = "1",
                DoctorId = "2"
            };
            _mockRepo.Setup(x => x.GetAppointmentById(appointmentId)).ReturnsAsync(appointmentReadModel);

            var query = new AppointmentRecordQuery
            {
                Id = appointmentId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Appointment.Should().BeEquivalentTo(appointmentReadModel);

            _mockRepo.Verify(x => x.GetAppointmentById(appointmentId), Times.Once);

        }
        finally
        {
            _mockRepo.Invocations.Clear();
        }
    }

    [Fact]
    public async Task Handle_NonExistingAppointment_ReturnsAppointmentRecordQueryResponseWithNullAppointment()
    {
        try
        {
            // Arrange
            var appointmentId = 1234;
            _mockRepo.Setup(x => x.GetAppointmentById(appointmentId)).ReturnsAsync((AppointmentReadModel)null);

            var query = new AppointmentRecordQuery
            {
                Id = appointmentId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Appointment.Should().BeNull();

            _mockRepo.Verify(x => x.GetAppointmentById(appointmentId), Times.Once);

        }
        finally
        {
            _mockRepo.Invocations.Clear();
        }
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenRepositoryIsNull()
    {
        // Arrange
        IAppointmentRepository nullRepository = null!;

        // Act and Assert
        Assert.ThrowsAsync<Exception>(() => new AppointmentRecordQueryHandler(nullRepository).Handle(new AppointmentRecordQuery(), CancellationToken.None));
    }
}
