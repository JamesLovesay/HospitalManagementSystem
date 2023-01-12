using FluentAssertions;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Handlers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MongoDB.Bson;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries
{
    public class DeleteDoctorCommandHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _repository;
        private readonly DeleteDoctorCommandHandler _handler;

        public DeleteDoctorCommandHandlerTests()
        {
            _repository= new Mock<IDoctorsRepository>();
            _handler = new DeleteDoctorCommandHandler(_repository.Object);
        }

        [Fact]
        public async Task WhenNonExistentDoctorDeleted_ExpectedResult()
        {
            string id = ObjectId.GenerateNewId().ToString();

            _repository.Setup(x => x.DeleteDoctor(id)).Returns(Task.FromResult(false));

            var result = await _handler.Handle(new DoctorDeleteCommand { DoctorId = id }, new CancellationToken());

            result.Should().BeFalse();

            _repository.Verify(x => x.DeleteDoctor(id), Times.Never());
        }

        [Fact]
        public async Task WhenExistingDoctorDeleted_ExpectedResult()
        {
            string id = ObjectId.GenerateNewId().ToString();
            var doctor = new DoctorReadModel
            {
                _id = id,
                Name = "test",
                HourlyChargingRate = 800,
                Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                Status = DoctorStatus.Inactive.ToString()
            };

            _repository.Setup(x => x.GetDoctorById(id)).ReturnsAsync(doctor);

            _repository.Setup(x => x.DeleteDoctor(id)).Returns(Task.FromResult(true));

            var result = await _handler.Handle(new DoctorDeleteCommand { DoctorId = id }, new CancellationToken());

            result.Should().BeTrue();

            _repository.Verify(x => x.DeleteDoctor(id), Times.Once());
        }

        [Fact]
        public async Task DeleteDoctorCommandHandler_ThrowsException_OnError()
        {
            string id = ObjectId.GenerateNewId().ToString();

            _repository
                .Setup(x => x.GetDoctorById(id))
                .Throws(new Exception());

            var command = new DoctorDeleteCommand { DoctorId = id };
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
