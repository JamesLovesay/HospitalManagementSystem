using FluentAssertions;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Api.Tests.Test_Helpers;
using MongoDB.Bson;
using Moq;

namespace HospitalManagementSystem.Api.Tests.Queries
{
    public class DoctorQueryHandlerTests
    {
        private readonly Mock<IDoctorsRepository> _repository = new Mock<IDoctorsRepository>();
        private readonly DoctorsQueryHandler _handler;

        public DoctorQueryHandlerTests()
        {
            _handler = new DoctorsQueryHandler(_repository.Object);
        }

        [Fact]
        public async Task WhenNoDoctorsFound_Null_ExpectedResult()
        {
            var doctorId = new ObjectId("456098909890987545678657");
            var status = nameof(DoctorStatus.ActiveVisiting);
            var specialism = nameof(DoctorSpecialism.Psychiatry);

            _repository.Setup(x => x.GetDoctors(It.Is<DoctorsQueryModel>(y => TestHelpers.IsEquivalent(y.DoctorIds, new List<ObjectId> { doctorId }))))
                .ReturnsAsync(((List<DoctorReadModel>)null));

            var result = await _handler.Handle(new DoctorsQuery { DoctorIds = new List<ObjectId> { doctorId }, Statuses = new List<string> { status }, Specialisms = new List<string> { specialism } },  new CancellationToken());

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(new DoctorsQueryResponse { Doctors = null });
        }

        [Fact]
        public async Task WhenNoDoctorRecordFound_Empty_ExpectedResult()
        {
            var doctorId = new ObjectId("456098909890987545678657");
            var status = nameof(DoctorStatus.ActiveVisiting);
            var specialism = nameof(DoctorSpecialism.Psychiatry);

            _repository.Setup(x => x.GetDoctors(It.Is<DoctorsQueryModel>(y => TestHelpers.IsEquivalent(y.DoctorIds, new List<ObjectId> { doctorId }))))
                .ReturnsAsync(new List<DoctorReadModel>());

            var result = await _handler.Handle(new DoctorsQuery { DoctorIds = new List<ObjectId> { doctorId }, Statuses = new List<string> { status }, Specialisms = new List<string> { specialism } }, new CancellationToken());

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
            });
        }


        [Fact]
        public async Task WhenJobRecordFound_ExpectedResult()
        {
            var doctor1Id = new ObjectId("456098909890987545678657");
            var status1 = DoctorStatus.ActiveVisiting;
            var specialism1 = DoctorSpecialism.Psychiatry;

            var doctor2Id = new ObjectId("456098909890987545668657");
            var status2 = DoctorStatus.Inactive;
            var specialism2 = DoctorSpecialism.Orthopaedics;

            var returnedDoctorReadModel = new List<DoctorReadModel>
            {
                new DoctorReadModel
                {
                        _id = doctor1Id,
                        DoctorId= doctor1Id,
                        Name = "Dr A",
                        HourlyChargingRate = 100,
                        Status = status1,
                        Specialism = specialism1
                },
                new DoctorReadModel
                {
                        _id = doctor2Id,
                        DoctorId= doctor2Id,
                        Name = "Dr B",
                        HourlyChargingRate = 300,
                        Status = status2,
                        Specialism = specialism2
                }
            };

            var returnedDoctorModel = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId= doctor1Id,
                    Name = "Dr A",
                    HourlyChargingRate = 100,
                    Status = status1,
                    Specialism = specialism1
                },
                new Doctor
                {
                    DoctorId= doctor2Id,
                    Name = "Dr B",
                    HourlyChargingRate = 300,
                    Status = status2,
                    Specialism = specialism2
                },
            };

            _repository.Setup(x => x.GetDoctors(It.Is<DoctorsQueryModel>(y => TestHelpers.IsEquivalent(y.DoctorIds, new List<ObjectId> { doctor1Id, doctor2Id }))))
                .ReturnsAsync(returnedDoctorReadModel);

            var result = await _handler.Handle(new DoctorsQuery { DoctorIds = new List<ObjectId> { doctor1Id, doctor2Id }, Statuses = new List<string> { nameof(status1), nameof(status2) }, Specialisms = new List<string> { nameof(specialism1), nameof(specialism2) } }, new CancellationToken());

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = returnedDoctorModel,
            });
        }
    }
}
