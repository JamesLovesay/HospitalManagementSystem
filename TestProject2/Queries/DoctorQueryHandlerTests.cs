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
        public async Task WhenNoDoctorRecordFound_Empty_ExpectedResult()
        {
            var doctorId = new ObjectId("456098909890987545678657");
            var status = nameof(DoctorStatus.ActiveVisiting);
            var specialism = nameof(DoctorSpecialism.Psychiatry);

            _repository.Setup(x => x.GetDoctors(It.Is<DoctorsQueryModel>(y => TestHelpers.IsEquivalent(y.DoctorId, new List<ObjectId> { doctorId }))))
                .ReturnsAsync((new List<DoctorReadModel>(), new DoctorsQueryDetail { Page = 1, PageSize = 2, TotalPages = 0, TotalRecords = 0 }));

            var result = await _handler.Handle(new DoctorsQuery { DoctorId = new List<ObjectId> { doctorId }, Status = new List<string> { status }, Specialism = new List<string> { specialism } }, new CancellationToken());

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
                Detail = new DoctorsQueryDetail
                {
                    Page = 1,
                    PageSize = 2,
                    TotalPages = 0,
                    TotalRecords = 0,
                }
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
                    DoctorId= doctor1Id.ToString(),
                    Name = "Dr A",
                    HourlyChargingRate = 100,
                    Status = status1,
                    Specialism = specialism1
                },
                new Doctor
                {
                    DoctorId= doctor2Id.ToString(),
                    Name = "Dr B",
                    HourlyChargingRate = 300,
                    Status = status2,
                    Specialism = specialism2
                },
            };

            _repository.Setup(x => x.GetDoctors(It.Is<DoctorsQueryModel>(y => TestHelpers.IsEquivalent(y.DoctorId, new List<ObjectId> { doctor1Id, doctor2Id }))))
                .ReturnsAsync((returnedDoctorReadModel, new DoctorsQueryDetail { Page = 1, PageSize = 20, TotalPages = 1, TotalRecords = 2 }));

            var result = await _handler.Handle(new DoctorsQuery { DoctorId = new List<ObjectId> { doctor1Id, doctor2Id }, Status = new List<string> { nameof(status1), nameof(status2) }, Specialism = new List<string> { nameof(specialism1), nameof(specialism2) } }, new CancellationToken());

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = returnedDoctorModel,
                Detail = new DoctorsQueryDetail
                {
                    Page = 1,
                    PageSize = 20,
                    TotalPages = 1,
                    TotalRecords = 2
                }
            });
        }
    }
}
