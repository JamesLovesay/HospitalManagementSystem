using FluentAssertions;
using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Patients;
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
                    DateOfBirth = "1984-01-01",
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

    #region Upsert Patient

    [Fact]
    public async Task UpsertPatient_NewPatient_InsertsPatientIntoDatabase()
    {
        // Arrange
        var newPatient = new PatientReadModel { _id = "123", Name = "John Doe", Gender = "Male", PhoneNumber = "1234567890" };

        // Act
        await _repository.UpsertPatient(newPatient);
        var result = await _repository.GetPatientById("123");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newPatient);
    }

    [Fact]
    public async Task UpsertPatient_ExistingPatient_UpdatesPatientInDatabase()
    {
        // Arrange
        var priorResult = await _repository.GetPatients(new PatientsQueryModel { isAdmitted = true });
        priorResult.patients.Should().Contain(p => p._id == patientId1.ToString());

        var updatedPatient = new PatientReadModel { _id = patientId1.ToString(), Name = "Jane Doe", Gender = "Female", PhoneNumber = "0987654321" };

        // Act
        await _repository.UpsertPatient(updatedPatient);
        var result = await _repository.GetPatientById(patientId1.ToString());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedPatient, options => options
            .Excluding(x => x.DateOfBirth)
            .Excluding(x => x.Email)
            .Excluding(x => x.PatientStatus)
            .Excluding(x => x.AdmissionDate)
            .Excluding(x => x.RoomId)
        );
    }

    [Fact]
    public async Task UpsertPatient_NullCommand_ThrowsArgumentNullException()
    {
        // Arrange
        PatientReadModel nullCommand = null!;

        // Act
        Func<Task> action = async () => await _repository.UpsertPatient(nullCommand);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    #endregion

    #region Get Patients Query

    [Fact]
    public async Task GetPatients_ShouldReturnPatientsAndDetail_WhenCalledWithQuery()
    {
        // Arrange
        var query = new PatientsQueryModel
        {
            Page = 1,
            PageSize = 10,
            Gender = "Male",
            isAdmitted = true,
        };

        var expectedDetail = new PatientsQueryDetail
        {
            Page = 1,
            PageSize = 10,
            TotalRecords = 2,
            TotalPages = 1,
            SortBy = "Name",
            SortDirection = "DESC",
        };

        // Act
        var (patients, detail) = await _repository.GetPatients(query);

        // Assert
        patients.Should().BeEquivalentTo(new List<PatientReadModel>()
        {
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
            }
        });
        detail.Should().BeEquivalentTo(expectedDetail);
    }

    [Fact]
    public void GetPatients_ShouldThrowArgumentNullException_WhenCalledWithNullQuery()
    {
        // Arrange
        PatientsQueryModel query = null!;

        // Act
        Func<Task> act = async () => await _repository.GetPatients(query);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetPatients_ShouldReturnDefaultPageSize_WhenPageSizeIsNull()
    {
        // Arrange
        var query = new PatientsQueryModel
        {
            Page = 1,
            Gender = "Female",
            isAdmitted = false,
        };

        var expectedDetail = new PatientsQueryDetail
        {
            Page = 1,
            PageSize = QueryHelper.DefaultPageSize,
            TotalRecords = 1,
            TotalPages = 1,
            SortBy = "Name",
            SortDirection = "DESC",
        };

        // Act
        var (patients, detail) = await _repository.GetPatients(query);

        // Assert
        patients.Should().BeEquivalentTo(new List<PatientReadModel>()
        {
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
            }
        });
        detail.Should().BeEquivalentTo(expectedDetail);
    }

    [Fact]
    public async Task GetPatients_ShouldReturnFirstPage_WhenPageIsLessThanOne()
    {
        // Arrange
        var query = new PatientsQueryModel
        {
            Page = 0,
            PageSize = 1,
            Name = "Example TestB",
            isAdmitted = true
        };

        var expectedDetail = new PatientsQueryDetail
        {
            Page = 1,
            PageSize = 1,
            TotalRecords = 1,
            TotalPages = 1,
            SortBy = "Name",
            SortDirection = "DESC",
            Name = "Example TestB",
        };

        // Act
        var (patients, detail) = await _repository.GetPatients(query);

        // Assert
        patients.Should().BeEquivalentTo(new List<PatientReadModel>()
        {
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
            }
        });
        detail.Should().BeEquivalentTo(expectedDetail);
    }

    [Fact]
    public async Task GetPatients_ShouldReturnEmptyList_WhenNoRecordsFound()
    {
        // Arrange
        var query = new PatientsQueryModel
        {
            Page = 1,
            PageSize = 10,
            Name = "Unknown",
        };

        var expectedDetail = new PatientsQueryDetail
        {
            Page = 1,
            PageSize = 10,
            TotalRecords = 0,
            TotalPages = 0,
            SortBy = "Name",
            SortDirection = "DESC",
            Name = "Unknown"
        };

        // Act
        var (patients, detail) = await _repository.GetPatients(query);

        // Assert
        patients.Should().BeEmpty();
        detail.Should().BeEquivalentTo(expectedDetail);
    }

    [Fact]
    public async Task GetPatients_ShouldReturnPaginatedPatients_WhenPageSizeIsLessThanTotalRecords()
    {
        // Arrange
        var query = new PatientsQueryModel
        {
            Page = 1,
            PageSize = 1
        };

        var expectedDetail = new PatientsQueryDetail
        {
            Page = 1,
            PageSize = 1,
            TotalRecords = 2,
            TotalPages = 2,
            SortBy = "Name",
            SortDirection = "DESC"
        };

        // Act
        var (patients, detail) = await _repository.GetPatients(query);

        // Assert
        patients.Should().HaveCount(1);
        detail.Should().BeEquivalentTo(expectedDetail);
    }

    [Fact]
    public async Task GetPatients_ShouldReturnAllPatients_WhenPageSizeIsGreaterThanTotalRecords()
    {
        // Arrange
        var query = new PatientsQueryModel
        {
            Page = 1,
            PageSize = 10,
            isAdmitted = true
        };

        var expectedDetail = new PatientsQueryDetail
        {
            Page = 1,
            PageSize = 10,
            TotalRecords = 4,
            TotalPages = 1,
            SortBy = "Name",
            SortDirection = "DESC"
        };

        // Act
        var (patients, detail) = await _repository.GetPatients(query);

        // Assert
        patients.Should().HaveCount(4);
        detail.Should().BeEquivalentTo(expectedDetail);
    }

    [Fact]
    public async Task GetPatients_ShouldReturnDefaultPage_WhenPageIsNull()
    {
        // Arrange
        var query = new PatientsQueryModel
        {
            PageSize = 10,
            isAdmitted = false
        };

        var expectedDetail = new PatientsQueryDetail
        {
            Page = 1,
            PageSize = 10,
            TotalRecords = 2,
            TotalPages = 1,
            SortBy = "Name",
            SortDirection = "DESC",
        };

        // Act
        var (patients, detail) = await _repository.GetPatients(query);

        // Assert
        patients.Count().Should().Be(2);
        detail.Should().BeEquivalentTo(expectedDetail);
    }

    [Fact]
    public async Task GetPatients_ShouldReturnSortedPatients_WhenCalledWithSortDetails()
    {
        // Arrange
        var query = new PatientsQueryModel
        {
            Page = 1,
            PageSize = 10,
            SortBy = "DateOfBirth",
            SortDirection = "ASC",
            isAdmitted = true
        };

        // Act
        var (patients, detail) = await _repository.GetPatients(query);

        // Assert
        patients.Should().BeEquivalentTo(new List<PatientReadModel>()
        {
            new PatientReadModel
            {
                Name = "Example TestF",
                DateOfBirth = "1984-01-01",
                AdmissionDate = "2022-06-01",
                IsAdmitted = true,
                RoomId = 104,
                Gender = "Female",
                PatientStatus = "Inpatient",
                _id = patientId6.ToString(),
                Email = "testF@test.com",
                PhoneNumber = "4546765767677676"
            },
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
            }
        });

        detail.Should().NotBeNull();
    }

    #endregion
}
