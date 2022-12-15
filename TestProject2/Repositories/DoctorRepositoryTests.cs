using FluentAssertions;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Serilog;

namespace HospitalManagementSystem.Api.Tests.Repositories
{
    public class DoctorsRepositoryTests
    {
        private readonly IMongoCollection<DoctorReadModel>? _doctorCollection;
        private readonly DoctorsRepository _repository;

        private readonly ObjectId doctorId1 = new ObjectId("094354543459057938450398");
        private readonly ObjectId doctorId2 = new ObjectId("458094358094545845890988");
        private readonly ObjectId doctorId3 = new ObjectId("459358943854958495304859");
        private readonly ObjectId doctorId4 = new ObjectId("459435894308084989403850");
        private readonly ObjectId doctorId5 = new ObjectId("584935809438498594038594");
        private readonly ObjectId doctorId6 = new ObjectId("589403859489984932843900");

        public DoctorsRepositoryTests()
        {
            var mongoFactory = new Mock<IMongoFactory>();
            var runner = MongoDbRunner.Start();

            MongoClient client = new MongoClient(runner.ConnectionString);
            var database = client.GetDatabase("DoctorsRepositoryTests");
            mongoFactory.Setup(x => x.GetDatabase()).Returns(database);

            _doctorCollection = database.GetCollection<DoctorReadModel>(nameof(Doctor));
            _doctorCollection.InsertMany(new List<DoctorReadModel> {
                new DoctorReadModel
                {
                    Name = "Dr Test A",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.ActivePermanent,
                    HourlyChargingRate = 800,
                    DoctorId = doctorId1,
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test B",
                    Specialism = DoctorSpecialism.Psychiatry,
                    Status = DoctorStatus.ActiveVisiting,
                    HourlyChargingRate = 500,
                    DoctorId = doctorId2,
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test C",
                    Specialism = DoctorSpecialism.Neurology,
                    Status = DoctorStatus.Inactive,
                    HourlyChargingRate = 600,
                    DoctorId = doctorId3,
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Mr Test D",
                    Specialism = DoctorSpecialism.Psychology,
                    Status = DoctorStatus.ActivePermanent,
                    HourlyChargingRate = 700,
                    DoctorId = doctorId4,
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test E",
                    Specialism = DoctorSpecialism.Urology,
                    Status = DoctorStatus.ActiveVisiting,
                    HourlyChargingRate = 600,
                    DoctorId = doctorId5,
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.Inactive,
                    HourlyChargingRate = 750,
                    DoctorId = doctorId6,
                    _id = new MongoDB.Bson.ObjectId(),
                }
            });

            _repository = new DoctorsRepository(mongoFactory.Object, new Mock<ILogger>().Object);
        }


        [Fact]
        public async Task WhenGetDoctors_EmptyQuery_AllRecordsReturned()
        {
            (await _doctorCollection.FindAsync(Builders<DoctorReadModel>.Filter.Empty))
                .ToList()
                .Count()
                .Should().Be(6);

            var result = await _repository.GetDoctors(new DoctorsQueryModel { });

            result.Should().NotBeNull();
            result.doctors.Should().BeEquivalentTo(new List<DoctorReadModel> {
                new DoctorReadModel
                {
                    Name = "Dr Test A",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.ActivePermanent,
                    HourlyChargingRate = 800,
                },
                new DoctorReadModel
                {
                    Name = "Dr Test B",
                    Specialism = DoctorSpecialism.Psychiatry,
                    Status = DoctorStatus.ActiveVisiting,
                    HourlyChargingRate = 500,
                },
                new DoctorReadModel
                {
                    Name = "Dr Test C",
                    Specialism = DoctorSpecialism.Neurology,
                    Status = DoctorStatus.Inactive,
                    HourlyChargingRate = 600,
                },
                new DoctorReadModel
                {
                    Name = "Mr Test D",
                    Specialism = DoctorSpecialism.Psychology,
                    Status = DoctorStatus.ActivePermanent,
                    HourlyChargingRate = 700,
                },
                new DoctorReadModel
                {
                    Name = "Dr Test E",
                    Specialism = DoctorSpecialism.Urology,
                    Status = DoctorStatus.ActiveVisiting,
                    HourlyChargingRate = 600,
                },
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.Inactive,
                    HourlyChargingRate = 750,
                }
            }, options => options.Excluding(x => x._id).Excluding(x => x.DoctorId));
        }

        [Fact]
        public async Task WhenGetDoctors_QuerySingleSpecialism_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                Specialisms = new List<DoctorSpecialism> { DoctorSpecialism.Orthopaedics }
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(2);
            result.doctors.Should().BeEquivalentTo(new List<DoctorReadModel> {
                new DoctorReadModel
                {
                    Name = "Dr Test A",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.ActivePermanent,
                    HourlyChargingRate = 800,
                },
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.Inactive,
                    HourlyChargingRate = 750,
                }
            }, options => options.Excluding(x => x._id).Excluding(x => x.DoctorId));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryMultipleSpecialisms_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                Specialisms = new List<DoctorSpecialism> { DoctorSpecialism.Orthopaedics, DoctorSpecialism.Neurology }
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(3);

        }

        [Fact]
        public async Task WhenGetDoctors_QuerySingleStatus_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                Status = new List<DoctorStatus> { DoctorStatus.Inactive }
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(2);
            result.doctors.Should().BeEquivalentTo(new List<DoctorReadModel> {
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.Inactive,
                    HourlyChargingRate = 750,
                },
                new DoctorReadModel
                {
                    Name = "Dr Test C",
                    Specialism = DoctorSpecialism.Neurology,
                    Status = DoctorStatus.Inactive,
                    HourlyChargingRate = 600,
                }
            }, options => options.Excluding(x => x._id).Excluding(x => x.DoctorId));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryMultipleStatus_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                Status = new List<DoctorStatus> { DoctorStatus.Inactive, DoctorStatus.ActivePermanent }
            };
            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(4);
        }

        [Fact]
        public async Task WhenGetDoctors_QueryName_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                Name = "Dr Test A"
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(1);
            result.doctors.Should().BeEquivalentTo(new List<DoctorReadModel>()
            {
                new DoctorReadModel
                {
                    Name = "Dr Test A",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.ActivePermanent,
                    HourlyChargingRate = 800,
                }
            }, options => options.Excluding(x => x._id).Excluding(x => x.DoctorId));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryDoctorId_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                DoctorId = new List<ObjectId>() { doctorId1 }
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(1);
            result.doctors.Should().BeEquivalentTo(new List<DoctorReadModel>()
            {
                new DoctorReadModel
                {
                    DoctorId = doctorId1,
                    Name = "Dr Test A",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.ActivePermanent,
                    HourlyChargingRate = 800,
                }
            }, options => options.Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryMultipleDoctorIds_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                DoctorId = new List<ObjectId>() { doctorId1, doctorId2 }
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(2);
        }

        [Fact]
        public async Task WhenGetDoctors_QuerySortBySpecialismAscending_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                SortBy = "Specialism",
                SortDirection = "ASC"
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(6);
            result.doctors[0].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test C",
                Specialism = DoctorSpecialism.Neurology,
                Status = DoctorStatus.Inactive,
                HourlyChargingRate = 600,
                DoctorId = doctorId3,
            }, options => options.Excluding(x => x._id));

            result.doctors[5].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test E",
                Specialism = DoctorSpecialism.Urology,
                Status = DoctorStatus.ActiveVisiting,
                HourlyChargingRate = 600,
                DoctorId = doctorId5,
            }, options => options.Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QuerySortByStatusDescending_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                SortBy = "Status",
                SortDirection = "DESC"
            };
            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(6);
            result.doctors[0].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test C",
                Specialism = DoctorSpecialism.Neurology,
                Status = DoctorStatus.Inactive,
                HourlyChargingRate = 600,
                DoctorId = doctorId3,
            }, options => options.Excluding(x => x._id));

            result.doctors[5].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Mr Test D",
                Specialism = DoctorSpecialism.Psychology,
                Status = DoctorStatus.ActivePermanent,
                HourlyChargingRate = 700,
                DoctorId = doctorId4,
            }, options => options.Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QuerySortByHourlyRateAscending_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                SortBy = "HourlyChargingRate",
                SortDirection = "ASC",
            };
            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(6);
            result.doctors[0].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test B",
                Specialism = DoctorSpecialism.Psychiatry,
                Status = DoctorStatus.ActiveVisiting,
                HourlyChargingRate = 500,
                DoctorId = doctorId2,
            }, options => options.Excluding(x => x._id));

            result.doctors[5].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test A",
                Specialism = DoctorSpecialism.Orthopaedics,
                Status = DoctorStatus.ActivePermanent,
                HourlyChargingRate = 800,
                DoctorId = doctorId1,
            }, options => options.Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryPageSize_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                PageSize = 4
            };
            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(4);
            result.doctors[3].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test C",
                Specialism = DoctorSpecialism.Neurology,
                Status = DoctorStatus.Inactive,
                HourlyChargingRate = 600,
                DoctorId = doctorId3,
            }, options => options.Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryPageNumberPageSize_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                PageSize = 2,
                Page = 2,
                SortBy = "HourlyChargingRate",
                SortDirection = "DESC"
            };
            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(2);
            result.doctors[0].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Mr Test D",
                Specialism = DoctorSpecialism.Psychology,
                Status = DoctorStatus.ActivePermanent,
                HourlyChargingRate = 700,
                DoctorId = doctorId4,
            }, options => options.Excluding(x => x._id));

            result.doctors[1].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test E",
                Specialism = DoctorSpecialism.Urology,
                Status = DoctorStatus.ActiveVisiting,
                HourlyChargingRate = 600,
                DoctorId = doctorId5,
            }, options => options.Excluding(x => x._id));
        }
    }
}
