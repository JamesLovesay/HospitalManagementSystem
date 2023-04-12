using FluentAssertions;
using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Handlers.Patients;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MongoDB.Bson;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries.Patients
{
    public class CreatePatientComandHandlerTests
    {
        private readonly Mock<IPatientsRepository> _repository;
        private readonly CreatePatientCommandHandler _handler;

        public CreatePatientComandHandlerTests()
        {
            _repository = new Mock<IPatientsRepository>();
            _handler = new CreatePatientCommandHandler(_repository.Object);
        }

        [Fact]
        public async Task WhenValidPatientCreated_ThenReturnPatientId()
        {
            try
            {
                // Arrange
                var expectedPatientId = ObjectId.GenerateNewId();
                _repository.Setup(x => x.GetPatients(It.IsAny<PatientsQueryModel>())).ReturnsAsync((new List<PatientReadModel>(), new PatientsQueryDetail()));
                _repository.Setup(x => x.UpsertPatient(It.IsAny<PatientReadModel>())).Returns(Task.FromResult(expectedPatientId));

                var command = new CreatePatientCommand
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                    Gender = "Male",
                    PhoneNumber = "07770123456",
                    IsAdmitted = true,
                };

                // Act
                var result = await _handler.Handle(command, CancellationToken.None);

                // Assert
                result.Should().NotBeNull();
                result.GetType().Should().Be(typeof(ObjectId));
                _repository.Verify(x => x.UpsertPatient(It.IsAny<PatientReadModel>()), Times.Once);
            }
            finally
            {
                _repository.Invocations.Clear();
            }

        }

        [Fact]
        public void WhenExistingPatientCreated_ThenThrowPatientExistsException()
        {
            try
            {
                // Arrange
                var existingPatient = new PatientReadModel
                {
                    Name = "John Doe",
                    DateOfBirth = new DateTime(1990, 01, 01).ToString()
                };

                var patientsResult = new List<PatientReadModel>
            {
                existingPatient
            };

                _repository.Setup(x => x.GetPatients(It.IsAny<PatientsQueryModel>())).ReturnsAsync((patientsResult, new PatientsQueryDetail()));

                var command = new CreatePatientCommand
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                    Gender = "Male",
                    PhoneNumber = "07770123456",
                    IsAdmitted = true,
                };

                // Act
                Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

                // Assert
                action.Should().ThrowAsync<PatientExistsException>().WithMessage("Patient with this name already exists.");
            }
            finally
            {
                _repository.Invocations.Clear();
            }
        }

        [Fact]
        public void WhenErrorOccurs_ThenThrowException()
        {
            try
            {
                // Arrange
                var command = new CreatePatientCommand
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                    Gender = "Male",
                    PhoneNumber = "07770123456",
                    IsAdmitted = true,
                };

                _repository.Setup(x => x.GetPatients(It.IsAny<PatientsQueryModel>())).ThrowsAsync(new Exception("An error occurred."));

                // Act
                Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

                // Assert
                action.Should().ThrowAsync<Exception>().WithMessage("Error when creating patient John Doe");
            }
            finally 
            { 
                _repository.Invocations.Clear();
            }
        }
    }
}
