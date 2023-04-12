using FluentAssertions;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MongoDB.Bson;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries.Doctors
{
    public class DoctorRecordQueryHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _repository = new Mock<IDoctorsRepository>();
        private readonly DoctorRecordQueryHandler _handler;

        public DoctorRecordQueryHandlerTests()
        {
            _handler = new DoctorRecordQueryHandler(_repository.Object);
        }

        [Fact]
        public async Task WhenNoRecordFound_Empty_ExpectedResult()
        {
            var doctorId = ObjectId.GenerateNewId();

            _repository.Setup(x => x.GetDoctorById(It.Is<string>(y => y == doctorId.ToString())))
                .ReturnsAsync(new DoctorReadModel());

            var result = await _handler.Handle(new DoctorRecordQuery { DoctorId = doctorId.ToString() }, new CancellationToken());

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(new DoctorRecordQueryResponse
            {
                DoctorId = null,
            });

            result.NotFoundInReadStore().Should().BeTrue();
            result.IsReady().Should().BeFalse();
        }

        [Fact]
        public async Task WhenRecordFound_ExpectedResult()
        {
            var doctorId = ObjectId.GenerateNewId();

            _repository.Setup(x => x.GetDoctorById(It.Is<string>(y => y == doctorId.ToString())))
                .ReturnsAsync(new DoctorReadModel { _id = doctorId.ToString() });

            var result = await _handler.Handle(new DoctorRecordQuery { DoctorId = doctorId.ToString() }, new CancellationToken());

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(new DoctorRecordQueryResponse
            {
                DoctorId = doctorId.ToString(),
            });

            result.NotFoundInReadStore().Should().BeFalse();
            result.IsReady().Should().BeTrue();
        }
    }
}
