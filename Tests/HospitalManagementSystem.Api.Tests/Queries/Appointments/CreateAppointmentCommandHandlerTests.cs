using FluentAssertions;
using HospitalManagementSystem.Api.Commands.Appointments;
using HospitalManagementSystem.Api.Events.Appointments;
using HospitalManagementSystem.Api.Handlers.Appointments;
using HospitalManagementSystem.Api.Models.Appointments;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries.Appointments;

public class CreateAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentRepository> _repository;
    private readonly Mock<IMediator> _mediator;
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _repository = new Mock<IAppointmentRepository>();
        _mediator = new Mock<IMediator>();
        _handler = new CreateAppointmentCommandHandler(_repository.Object, _mediator.Object);
    }

    [Fact]
    public async Task WhenValidAppointmentCreated_ThenReturnAppointmentId()
    {
        try
        {
            // Arrange
            var expectedAppointmentId = Guid.NewGuid();
            _repository.Setup(x => x.AddAppointment(It.IsAny<Appointment>())).ReturnsAsync(expectedAppointmentId);

            var command = new CreateAppointmentCommand
            {
                Description = "Checkup",
                PatientId = 1,
                DoctorId = 2,
                StartTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                EndTime = DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                DoctorName = "John Doe",
                PatientName = "Jane Doe",
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            result.GetType().Should().Be(typeof(Guid));
            _repository.Verify(x => x.AddAppointment(It.IsAny<Appointment>()), Times.Once);
            _mediator.Verify(x => x.Publish(It.IsAny<AppointmentCreatedEvent>(), CancellationToken.None), Times.Once);
        }
        finally
        {
            _repository.Invocations.Clear();
            _mediator.Invocations.Clear();
        }
    }

    [Fact]
    public async Task WhenInvalidAppointmentCreated_ThenThrowValidationException()
    {
        // Arrange
        var command = new CreateAppointmentCommand
        {
            // empty command
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
