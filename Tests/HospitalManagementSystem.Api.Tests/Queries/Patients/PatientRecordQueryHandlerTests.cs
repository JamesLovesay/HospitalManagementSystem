using FluentAssertions;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries.Patients;

public class PatientRecordQueryHandlerTests
{
    private readonly Mock<IPatientsRepository> _mockRepo;
    private readonly PatientRecordQueryHandler _handler;

    public PatientRecordQueryHandlerTests()
    {
        _mockRepo = new Mock<IPatientsRepository>();
        _handler = new PatientRecordQueryHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ExistingPatient_ReturnsPatientRecordQueryResponse()
    {
        // Arrange
        var patientId = "1";
        var patientReadModel = new PatientReadModel
        {
            _id = patientId,
            Name = "Jane Smith",
            DateOfBirth = "01/01/1980",
            AdmissionDate = "01/01/2022",
            Gender = "Female",
            PatientStatus = "Admitted",
            PhoneNumber = "0987654321",
            Email = "janesmith@example.com",
            IsAdmitted = true,
            RoomId = 2
        };
        _mockRepo.Setup(x => x.GetPatientById(patientId)).ReturnsAsync(patientReadModel);

        var query = new PatientRecordQuery
        {
            PatientId = patientId
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PatientId.Should().Be(patientId);
        result.PatientName.Should().Be(patientReadModel.Name);
        result.DateOfBirth.Should().Be(patientReadModel.DateOfBirth);
        result.AdmissionDate.Should().Be(patientReadModel.AdmissionDate);
        result.Gender.Should().Be(patientReadModel.Gender);
        result.PatientStatus.Should().Be(patientReadModel.PatientStatus);
        result.PhoneNumber.Should().Be(patientReadModel.PhoneNumber);
        result.EmailAddress.Should().Be(patientReadModel.Email);
        result.IsAdmitted.Should().Be(patientReadModel.IsAdmitted);
        result.RoomId.Should().Be(patientReadModel.RoomId);
    }

    [Fact]
    public async Task Handle_NonExistingPatient_ReturnsEmptyPatientRecordQueryResponse()
    {
        // Arrange
        var patientId = "1";
        _mockRepo.Setup(x => x.GetPatientById(patientId)).ReturnsAsync((PatientReadModel?)null);

        var query = new PatientRecordQuery
        {
            PatientId = patientId
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.NotFoundInReadStore().Should().BeTrue();
    }

    [Fact]
    public async Task Handle_NullRepository_ThrowsArgumentNullException()
    {
        // Arrange
        var handler = new PatientRecordQueryHandler(null);

        var query = new PatientRecordQuery
        {
            PatientId = "1"
        };

        // Act
        Func<Task<PatientRecordQueryResponse>> act = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }
}
