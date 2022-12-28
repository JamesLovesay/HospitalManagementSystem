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
    public class CreateDoctorCommandHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _repository = new Mock<IDoctorsRepository>();
        private readonly CreateDoctorCommandHandler _handler;

        public CreateDoctorCommandHandlerTests()
        {
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
    }
}
