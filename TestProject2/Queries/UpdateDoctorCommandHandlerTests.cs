using FluentAssertions;
using HospitalManagementSystem.Api.Handlers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var result = await _handler.Handle(new Commands.DoctorUpdateCommand { DoctorId = id, HourlyChargingRate = 800 }, new CancellationToken());

            result.Should().BeTrue();
        }

        [Fact]
        public async Task WhenNonExistingDoctorUpdated_ThenExpectedResult()
        {
            var id = ObjectId.GenerateNewId().ToString();

            var result = await _handler.Handle(new Commands.DoctorUpdateCommand { DoctorId = id, HourlyChargingRate = 800 }, new CancellationToken());

            result.Should().BeFalse();
        }
    }
}
