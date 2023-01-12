using FluentAssertions;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Handlers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MongoDB.Bson;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries
{
    public class UpdateDoctorCommandHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _repository;
        private readonly UpdateDoctorCommandHandler _handler;

        public UpdateDoctorCommandHandlerTests()
        {
            _repository = new Mock<IDoctorsRepository>();
            _handler = new UpdateDoctorCommandHandler(_repository.Object);
        }

        [Fact]
        public async Task WhenExistingDoctorUpdated_ThenExpectedResult()
        {
            var id = ObjectId.GenerateNewId().ToString();
            DoctorReadModel doctor = new DoctorReadModel
            {
                _id = id,
                HourlyChargingRate = 900,
                Name = "name",
                Specialism = "Orthopaedics",
                Status = "Inactive"
            };

            _repository.Setup(x => x.GetDoctorById(id)).ReturnsAsync(doctor);

            _repository.Setup(x => x.UpsertDoctor(doctor)).Returns(Task.FromResult(id));

            var result = await _handler.Handle(new Commands.DoctorUpdateCommand { DoctorId = id, HourlyChargingRate = 800, Name = "name", Status = "Inactive"}, new CancellationToken());

            result.Should().BeTrue();

            _repository.Verify(x => x.UpsertDoctor(It.Is<DoctorReadModel>(x =>
                x.Name == "name" &&
                x.Status == "Inactive" &&
                x.HourlyChargingRate == 800)), Times.Once);
        }

        [Fact]
        public async Task WhenExistingDoctor_DoctorNotUpdated_ThenExpectedResult()
        {
            var id = ObjectId.GenerateNewId().ToString();
            DoctorReadModel doctor = new DoctorReadModel
            {
                _id = id,
                HourlyChargingRate = 900,
                Name = "name",
                Specialism = "Orthopaedics",
                Status = "Inactive"
            };

            _repository.Setup(x => x.GetDoctorById(id)).ReturnsAsync(doctor);

            _repository.Setup(x => x.UpsertDoctor(doctor)).Returns(Task.FromResult(id));

            var result = await _handler.Handle(new Commands.DoctorUpdateCommand { DoctorId = id, HourlyChargingRate = null }, new CancellationToken());

            result.Should().BeTrue();

            _repository.Verify(x => x.UpsertDoctor(It.IsAny<DoctorReadModel>()), Times.Once);
        }

        [Fact]
        public async Task WhenNonExistingDoctorUpdated_ThenExpectedResult()
        {
            var id = ObjectId.GenerateNewId().ToString();

            var result = await _handler.Handle(new Commands.DoctorUpdateCommand { DoctorId = id, HourlyChargingRate = 800 }, new CancellationToken());

            result.Should().BeFalse();
        }


        [Fact]
        public async Task UpdateDoctorCommandHandler_ThrowsException_OnError()
        {
            string id = ObjectId.GenerateNewId().ToString();

            _repository
                .Setup(x => x.GetDoctorById(id))
                .Throws(new Exception());

            var command = new DoctorUpdateCommand { DoctorId = id };
            bool result = false;

            try
            {
                result = await _handler.Handle(command, CancellationToken.None);
            }
            catch (Exception e) { }

            Assert.True(result == false);
        }
    }
}
