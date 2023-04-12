using FluentAssertions;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Serilog;

namespace HospitalManagementSystem.Api.Tests.Repositories;

public class PatientRepositoryTests
{
    private IMongoCollection<PatientReadModel>? _patientCollection;
    private IPatientsRepository _repository;

    private readonly ObjectId patientId1 = new ObjectId("094354543459057938450398");
    private readonly ObjectId patientId2 = new ObjectId("458094358094545845890988");
    private readonly ObjectId patientId3 = new ObjectId("459358943854958495304859");
    private readonly ObjectId patientId4 = new ObjectId("459435894308084989403850");
    private readonly ObjectId patientId5 = new ObjectId("584935809438498594038594");
    private readonly ObjectId patientId6 = new ObjectId("589403859489984932843900");

    public PatientRepositoryTests()
    {
        var mongoFactory = new Mock<IMongoFactory>();
        var runner = MongoDbRunner.Start();

        MongoClient client = new MongoClient(runner.ConnectionString);
        var database = client.GetDatabase("PatientsRepositoryTests");
        mongoFactory.Setup(x => x.GetDatabase()).Returns(database);

        _patientCollection = database.GetCollection<PatientReadModel>(nameof(PatientReadModel));
        _patientCollection.InsertMany(new List<PatientReadModel> {
                new PatientReadModel
                {
                    Name = "Example TestA",
                    DateOfBirth = "1989-01-01",
                    AdmissionDate = "2022-01-01",
                    IsAdmitted = true,
                    RoomId = 101,
                    Gender = "Male",
                    PatientStatus = "Inpatient",
                    _id = patientId1.ToString(),
                    Email = "testA@test.com",
                    PhoneNumber = "45465467657676"
                },
                new PatientReadModel
                {
                    Name = "Example TestB",
                    DateOfBirth = "1990-01-01",
                    AdmissionDate = "2022-02-01",
                    IsAdmitted = true,
                    RoomId = 102,
                    Gender = "Female",
                    PatientStatus = "Inpatient",
                    _id = patientId2.ToString(),
                    Email = "testB@test.com",
                    PhoneNumber = "45465446557676"
                },
                new PatientReadModel
                {
                    Name = "Example TestC",
                    DateOfBirth = "1991-01-01",
                    AdmissionDate = "2022-03-01",
                    IsAdmitted = true,
                    RoomId = 103,
                    Gender = "Male",
                    PatientStatus = "Inpatient",
                    _id = patientId3.ToString(),
                    Email = "testC@test.com",
                    PhoneNumber = "454667657677676"
                },
                new PatientReadModel
                {
                    Name = "Example TestD",
                    DateOfBirth = "1992-01-01",
                    AdmissionDate = "2022-04-01",
                    IsAdmitted = false,
                    Gender = "Female",
                    PatientStatus = "Outpatient",
                    _id = patientId4.ToString(),
                    Email = "testD@test.com",
                    PhoneNumber = "45465469879789676"
                },
                new PatientReadModel
                {
                    Name = "Example TestE",
                    DateOfBirth = "1993-01-01",
                    AdmissionDate = "2022-05-01",
                    IsAdmitted = false,
                    Gender = "Male",
                    PatientStatus = "Outpatient",
                    _id = patientId5.ToString(),
                    Email = "testE@test.com",
                    PhoneNumber = "454643243657676"
                },
                new PatientReadModel
                {
                    Name = "Example TestF",
                    DateOfBirth = "1994-01-01",
                    AdmissionDate = "2022-06-01",
                    IsAdmitted = true,
                    RoomId = 104,
                    Gender = "Female",
                    PatientStatus = "Inpatient",
                    _id = patientId6.ToString(),
                    Email = "testF@test.com",
                    PhoneNumber = "4546765767677676"
                }
            });

        _repository = new PatientsRepository(mongoFactory.Object, new Mock<ILogger>().Object);
    }

    #region Delete Patient

    [Fact]
    public async Task WhenDeletePatientById_PatientExists_ThenPatientIsDeleted()
    {
        //Arrange
        var priorResult = await _repository.GetPatients(new PatientsQueryModel { isAdmitted = true });
        priorResult.patients.Should().Contain(p => p._id == patientId1.ToString());

        // Act
        await _repository.DeletePatient(patientId1.ToString());

        // Assert
        var result = await _repository.GetPatients(new PatientsQueryModel { });
        result.patients.Should().NotContain(p => p._id == patientId1.ToString());
    }

    [Fact]
    public async Task DeletePatient_WithNonExistingPatientId_ShouldNotDeleteAnyRecordFromDatabase()
    {
        // Arrange
        var patientId = ObjectId.GenerateNewId().ToString();

        // Act
        await _repository.DeletePatient(patientId);

        // Assert
        var result = await _repository.GetPatientById(patientId.ToString());
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletePatient_WithNullPatientId_ShouldThrowArgumentNullException()
    {
        // Arrange
        string patientId = null!;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.DeletePatient(patientId));
    }

    [Fact]
    public async Task DeletePatient_WithEmptyPatientId_ShouldThrowArgumentNullException()
    {
        // Arrange
        var patientId = string.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.DeletePatient(patientId));
    }
    #endregion

    #region Get Patient By Id

    [Fact]
    public async Task GetPatientById_ValidId_ReturnsPatientReadModel()
    {
        // Act
        var result = await _repository.GetPatientById(patientId2.ToString());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new PatientReadModel
        {
            Name = "Example TestB",
            DateOfBirth = "1990-01-01",
            AdmissionDate = "2022-02-01",
            IsAdmitted = true,
            RoomId = 102,
            Gender = "Female",
            PatientStatus = "Inpatient",
            _id = patientId2.ToString(),
            Email = "testB@test.com",
            PhoneNumber = "45465446557676"
        });
    }

    [Fact]
    public async Task GetPatientById_InvalidId_ThrowsArgumentNullException()
    {
        // Arrange
        string invalidPatientId = null!;

        // Act
        Func<Task> action = async () => await _repository.GetPatientById(invalidPatientId);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetPatientById_PatientNotFound_ReturnsNull()
    {
        // Arrange
        var nonExistentPatientId = "nonexistentpatientid";

        // Act
        var result = await _repository.GetPatientById(nonExistentPatientId);

        // Assert
        result.Should().BeNull();
    }

    #endregion
}
