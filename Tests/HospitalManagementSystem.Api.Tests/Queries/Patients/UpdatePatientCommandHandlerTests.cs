using FluentAssertions;
using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Handlers.Patients;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using Moq;
using ZstdSharp.Unsafe;

namespace HospitalManagementSystem.Api.Tests.Queries.Patients;

public class UpdatePatientCommandHandlerTests
{
    private Mock<IPatientsRepository> _mockRepo;
    private UpdatePatientCommandHandler _handler;

    public UpdatePatientCommandHandlerTests()
    {
        _mockRepo = new Mock<IPatientsRepository>();
        _handler = new UpdatePatientCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ExistingPatient_ReturnsTrue()
    {
        try
        {
            // Arrange
            var command = new UpdatePatientCommand
            {
                PatientId = "1",
                Name = "John Doe",
                DateOfBirth = "01/01/1980",
                AdmissionDate = "01/01/2022",
                Gender = "Male",
                Status = "Recovered",
                PhoneNumber = "123-456-7890",
                Email = "johndoe@example.com",
                IsAdmitted = false,
                RoomId = 1
            };
            var patientReadModel = new PatientReadModel
            {
                _id = "1",
                Name = "Jane Smith",
                DateOfBirth = "01/01/1980",
                AdmissionDate = "01/01/2022",
                Gender = "Female",
                PatientStatus = "Admitted",
                PhoneNumber = "098-765-4321",
                Email = "janesmith@example.com",
                IsAdmitted = true,
                RoomId = 2
            };
            _mockRepo.Setup(x => x.GetPatientById(command.PatientId)).ReturnsAsync(patientReadModel);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            patientReadModel.Name.Should().Be("John Doe");
            patientReadModel.DateOfBirth.Should().Be("01/01/1980");
            patientReadModel.AdmissionDate.Should().Be("01/01/2022");
            patientReadModel.Gender.Should().Be("Male");
            patientReadModel.PatientStatus.Should().Be("Recovered");
            patientReadModel.PhoneNumber.Should().Be("123-456-7890");
            patientReadModel.Email.Should().Be("johndoe@example.com");
            patientReadModel.IsAdmitted.Should().BeFalse();
            patientReadModel.RoomId.Should().Be(1);
            _mockRepo.Verify(x => x.UpsertPatient(It.IsAny<PatientReadModel>()), Times.Once);
        }
        finally
        {
            _mockRepo.Invocations.Clear();
        }

    }

    [Fact]
    public async Task Handle_NonExistingPatient_ReturnsFalse()
    {
        try
        {
            // Arrange
            var command = new UpdatePatientCommand
            {
                PatientId = "1",
                Name = "John Doe",
                DateOfBirth = "01/01/1980",
                AdmissionDate = "01/01/2022",
                Gender = "Male",
                Status = "Recovered",
                PhoneNumber = "123-456-7890",
                Email = "johndoe@example.com",
                IsAdmitted = false,
                RoomId = 1
            };
            _mockRepo.Setup(x => x.GetPatientById(command.PatientId)).ReturnsAsync((PatientReadModel?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _mockRepo.Verify(x => x.UpsertPatient(It.IsAny<PatientReadModel>()), Times.Never);
        }
        finally
        {
            _mockRepo.Invocations.Clear();
        }

    }

    [Fact]
    public async Task Handle_ThrowsException_ReturnsFalse()
    {
        try
        {
            // Arrange
            var command = new UpdatePatientCommand
            {
                PatientId = "1",
                Name = "John Doe",
                DateOfBirth = "01/01/1980",
                AdmissionDate = "01/01/2022",
                Gender = "Male",
                Status = "Recovered",
                PhoneNumber = "1234567890",
                Email = "johndoe@example.com",
                IsAdmitted = false,
                RoomId = 1
            };
            _mockRepo.Setup(x => x.GetPatientById(command.PatientId)).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            _mockRepo.Verify(x => x.UpsertPatient(It.IsAny<PatientReadModel>()), Times.Never);
        }
        finally
        {
            _mockRepo.Invocations.Clear();
        }

    }

    [Fact]
    public async Task Handle_NullRepository_ThrowsException()
    {
        // Arrange
        var command = new UpdatePatientCommand
        {
            PatientId = "1",
            Name = "John Doe",
            DateOfBirth = "01/01/1980",
            AdmissionDate = "01/01/2022",
            Gender = "Male",
            Status = "Recovered",
            PhoneNumber = "123-456-7890",
            Email = "johndoe@example.com",
            IsAdmitted = false,
            RoomId = 1
        };
        var handler = new UpdatePatientCommandHandler(null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NullCommand_ThrowsException()
    {
        // Arrange
        var handler = new UpdatePatientCommandHandler(_mockRepo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(async () => await handler.Handle(null!, CancellationToken.None));
    }

}
