using FluentAssertions;
using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Handlers.Patients;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries.Patients;

public class DeletePatientCommandHandlerTests
{
    private readonly Mock<IPatientsRepository> _repositoryMock;
    private readonly DeletePatientCommandHandler _handler;

    public DeletePatientCommandHandlerTests()
    {
        _repositoryMock = new Mock<IPatientsRepository>();
        _handler = new DeletePatientCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenPatientExists_DeletesPatientAndReturnsTrue()
    {
        try
        {
            // Arrange
            var cmd = new DeletePatientCommand("1");
            _repositoryMock.Setup(repo => repo.GetPatientById("1")).ReturnsAsync(new PatientReadModel());

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeletePatient("1"), Times.Once);
            result.Should().BeTrue();
        }
        finally
        {
            _repositoryMock.Invocations.Clear();
        }

    }

    [Fact]
    public async Task Handle_WhenPatientNotFound_ReturnsFalse()
    {
        try
        {
            // Arrange
            var patientId = "1";
            var cmd = new DeletePatientCommand(patientId);
            _repositoryMock.Setup(repo => repo.GetPatientById(It.IsAny<string>())).Returns(Task.FromResult<PatientReadModel?>(null));

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _repositoryMock.Verify(repo => repo.DeletePatient(patientId), Times.Never);
        }
        finally
        {
            _repositoryMock.Invocations.Clear();
        }

    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_ThrowsExceptionWithCorrectMessage()
    {
        try
        {
            // Arrange
            var cmd = new DeletePatientCommand("1");
            _repositoryMock.Setup(repo => repo.GetPatientById("1")).ThrowsAsync(new Exception("Test Exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(cmd, CancellationToken.None));
            exception.Message.Should().Be($"Error whilst deleting patient {cmd.PatientId}");
            exception.InnerException.Should().BeNull();
        }
        finally
        {
            _repositoryMock.Invocations.Clear();
        }
    }
}
