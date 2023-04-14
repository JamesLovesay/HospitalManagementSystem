using FluentAssertions;
using HospitalManagementSystem.Api.Models.Appointments;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using Serilog;

namespace HospitalManagementSystem.Api.Tests.Repositories;

public class AppointmentsRepositoryTests
{
    private IMongoCollection<AppointmentReadModel>? _appointmentCollection;
    private IAppointmentRepository _repository;

    private readonly int appointmentId1 = 123656;
    private readonly int appointmentId2 = 196546;
    private readonly int appointmentId3 = 658986;
    private readonly int appointmentId4 = 105367;

    public AppointmentsRepositoryTests()
    {
        var mongoFactory = new Mock<IMongoFactory>();
        var runner = MongoDbRunner.Start();

        MongoClient client = new MongoClient(runner.ConnectionString);
        var database = client.GetDatabase("AppointmentsRepositoryTests");
        mongoFactory.Setup(x => x.GetDatabase()).Returns(database);

        _appointmentCollection = database.GetCollection<AppointmentReadModel>(nameof(AppointmentReadModel));
        _appointmentCollection.InsertMany(new List<AppointmentReadModel> {
                new AppointmentReadModel(appointmentId1, "2023-04-12T10:30:00")
                {
                    EndTime = "2023-04-12T11:30:00",
                    Description = "Appointment 1",
                    PatientId = "patientId1"
                },
                new AppointmentReadModel(appointmentId2, "2023-04-15T14:30:00")
                {
                    EndTime = "2023-04-15T15:30:00",
                    Description = "Appointment 2",
                    PatientId = "patientId2"
                },
                new AppointmentReadModel(appointmentId3, "2023-04-16T14:30:00")
                {
                    EndTime = "2023-04-16T14:45:00",
                    Description = "Appointment 3",
                    PatientId = "patientId3"
                },
                new AppointmentReadModel(appointmentId4, "2023-04-19T16:30:00")
                {
                    EndTime = "2023-04-19T17:00:00",
                    Description = "Appointment 4",
                    PatientId = "patientId1"
                }
            });

        _repository = new AppointmentsRepository(mongoFactory.Object, new Mock<ILogger>().Object);
    }

    #region Get Appointment By Id

    [Fact]
    public async Task WhenGetAppointmentById_AppointmentExists_ThenReturnsAppointment()
    {
        // Act
        var result = await _repository.GetAppointmentById(appointmentId1);

        // Assert
        result.Should().BeEquivalentTo(new AppointmentReadModel(appointmentId1, "2023-04-12T10:30:00")
        {
            EndTime = "2023-04-12T11:30:00",
            Description = "Appointment 1",
            PatientId = "patientId1"
        });
    }

    [Fact]
    public async Task WhenGetAppointmentById_AppointmentDoesNotExist_ThenReturnsNull()
    {
        // Act
        var result = await _repository.GetAppointmentById(new Random().Next());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task WhenGetAppointmentById_NegativeIdPassed_ThrowsArgumentException()
    {
        // Arrange
        int negativeId = -1;

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _repository.GetAppointmentById(negativeId));
    }

    #endregion

    #region Add Appointment

    //[Fact]
    //public async Task AddAppointment_ShouldInsertAppointmentIntoDatabase()
    //{
    //    // Arrange
    //    var appointmentId = Guid.NewGuid();
    //    var appointment = new Appointment (appointmentId, "Test Appointment", 2, "2022-12-12T02-02-02", "2022-12-12T02-02-02", "testDoctor", "testPatient", 5);

    //    // Act
    //    var result = await _repository.AddAppointment(appointment);

    //    // Assert
    //    result.Should().Be(appointment.Id);

    //    var returnedAppointment = await _repository.GetAppointmentById((int)appointmentId);

    //    returnedAppointment.Should().NotBeNull();
    //    returnedAppointment.Id.Should().Be(appointmentId);
    //    returnedAppointment.PatientName.Should().Be("testPatient");
    //}

    [Fact]
    public async Task AddAppointment_NullAppointment_ThrowsArgumentNullException()
    {
        // Arrange, Act, Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAppointment(null!));
    }

    [Fact]
    public async Task AddAppointment_ValidAppointment_ReturnsAppointmentId()
    {
        // Arrange
        var appointmentIdNumber = Guid.NewGuid();
        var appointment = new Appointment(appointmentIdNumber, "Test Appointment", 2, "2022-12-12T02-02-02", "2022-12-12T02-02-02", "testDoctor", "testPatient", 5);

        // Act
        var result = await _repository.AddAppointment(appointment);

        // Assert
        result.Should().NotBe(Guid.Empty);
        result.Should().Be(appointment.Id);
    }

    #endregion
}
