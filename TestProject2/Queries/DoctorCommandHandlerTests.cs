using FluentAssertions;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Handlers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries
{
    public class DoctorCommandHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _repository = new Mock<IDoctorsRepository>();
        private readonly DoctorCommandHandler _handler;

        public DoctorCommandHandlerTests()
        {
            _handler = new DoctorCommandHandler(_repository.Object);
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
        public async Task WhenDoctorDeleted_ExpectedResult()
        {
            string id = ObjectId.GenerateNewId().ToString();

            _repository.Setup(x => x.DeleteDoctor(id)).Returns(Task.FromResult(false));

            var result = await _handler.Handle(new DoctorDeleteCommand { DoctorId = id }, new CancellationToken());

            result.Should().BeFalse();
            result.GetType().Should().Be(typeof(Boolean));
        }
    }
}
