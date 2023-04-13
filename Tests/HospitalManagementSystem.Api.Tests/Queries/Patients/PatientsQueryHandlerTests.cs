using FluentAssertions;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries.Patients;

public class PatientsQueryHandlerTests
{
    private readonly Mock<IPatientsRepository> _mockRepo;
    private readonly PatientsQueryHandler _handler;

    public PatientsQueryHandlerTests()
    {
        _mockRepo = new Mock<IPatientsRepository>();
        _handler = new PatientsQueryHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ReturnsPatientsQueryResponse()
    {
        try
        {
            // Arrange
            var query = new PatientsQuery();
            var patientsQueryModel = new PatientsQueryModel(query);
            var patients = new List<PatientReadModel>
        {
            new PatientReadModel { _id = "1", Name = "John Doe", AdmissionDate = "2023-1-1", DateOfBirth = "1985-8-31", Email = "test@test.com", Gender = "male", IsAdmitted = true, PatientStatus = "InTreatment", PhoneNumber = "45645646546", RoomId = 2 },
            new PatientReadModel { _id = "2", Name = "Jane Smith" },
            new PatientReadModel { _id = "3", Name = "Bob Johnson" }
        };
            var detail = new PatientsQueryDetail
            {
                TotalRecords = 3,
                TotalPages = 1,
                Page = 1,
                PageSize = 20
            };
            _mockRepo.Setup(x => x.GetPatients(It.IsAny<PatientsQueryModel>())).ReturnsAsync((patients, detail));

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Patients.Should().NotBeNullOrEmpty();
            response.Detail.Should().NotBeNull();
            response.Patients.Should().HaveCount(patients.Count);
            response.Detail.TotalRecords.Should().Be(detail.TotalRecords);
            _mockRepo.Verify(x => x.GetPatients(It.IsAny<PatientsQueryModel>()), Times.Once);
        }
        finally
        {
            _mockRepo.Invocations.Clear();
        }

    }

    [Fact]
    public async Task Handle_NullRepository_ThrowsArgumentNullException()
    {
        // Arrange
        var query = new PatientsQuery();

        // Act
        Func<Task> act = async () =>
        {
            var handler = new PatientsQueryHandler(null!);
            await handler.Handle(query, CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>()
            .WithMessage("Object reference not set to an instance of an object.");
    }
}

