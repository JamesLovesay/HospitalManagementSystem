using FluentAssertions;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Handlers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MongoDB.Bson;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries
{
    public class CreateDoctorCommandHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _repository;
        private readonly CreateDoctorCommandHandler _handler;

        public CreateDoctorCommandHandlerTests()
        {
            _repository = new Mock<IDoctorsRepository>();
            _handler = new CreateDoctorCommandHandler(_repository.Object);
        }

        [Fact]
        public async Task WhenValidDoctorCreated_ExpectedResult()
        {
            ObjectId id = ObjectId.GenerateNewId();
            var doctor = new DoctorReadModel
            {
                _id = id.ToString(),
                Name = "test",
                HourlyChargingRate = 800,
                Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                Status = DoctorStatus.Inactive.ToString()
            };

            _repository.Setup(x => x.UpsertDoctor(doctor)).Returns(Task.FromResult(id));

            var result = await _handler.Handle(new CreateDoctorCommand { Name = "test", HourlyChargingRate = 800, Specialism = "Orthopaedics", Status = "Inactive" }, new CancellationToken());

            result.Should().NotBeNull();
            result.GetType().Should().Be(typeof(ObjectId));
        }

        [Fact]
        public async Task WhenDoctorExists_ThrowErrorExpectedResult()
        {
            ObjectId id = ObjectId.GenerateNewId();
            var doctor = new DoctorReadModel
            {
                _id = id.ToString(),
                Name = "test",
                HourlyChargingRate = 800,
                Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                Status = DoctorStatus.Inactive.ToString()
            };

            _repository.Setup(x => x.GetDoctors(It.IsAny<DoctorsQueryModel>()))
                .ReturnsAsync((new List<DoctorReadModel> { doctor }, new DoctorsQueryDetail { }));

            await Assert.ThrowsAsync<DoctorExistsException>(() => _handler.Handle(new CreateDoctorCommand { Name = "test", HourlyChargingRate = 800, Specialism = "Orthopaedics", Status = "Inactive" }, CancellationToken.None));
        }

        [Fact]
        public async Task CreateDoctorCommandHandler_ThrowsException_OnError()
        {
            _repository
                .Setup(x => x.GetDoctors(It.IsAny<DoctorsQueryModel>()))
                .Throws(new Exception());

            var command = new CreateDoctorCommand { Name = "name" };
            ObjectId? result = null;

            try
            {
                result = await _handler.Handle(command, CancellationToken.None);
            }
            catch (Exception e) { }

            result.Should().BeNull();
        }
    }
}
