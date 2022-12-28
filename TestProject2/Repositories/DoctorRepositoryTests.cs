using FluentAssertions;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Api.Repositories.Interfaces;
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
        private readonly IDoctorsRepository _repository;

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
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.ActivePermanent.ToString(),
                    HourlyChargingRate = 800,
                    _id = doctorId1.ToString(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test B",
                    Specialism = DoctorSpecialism.Psychiatry.ToString(),
                    Status = DoctorStatus.ActiveVisiting.ToString(),
                    HourlyChargingRate = 500,
                    _id = doctorId2.ToString(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test C",
                    Specialism = DoctorSpecialism.Neurology.ToString(),
                    Status = DoctorStatus.Inactive.ToString(),
                    HourlyChargingRate = 600,
                    _id = doctorId3.ToString(),
                },
                new DoctorReadModel
                {
                    Name = "Mr Test D",
                    Specialism = DoctorSpecialism.Psychology.ToString(),
                    Status = DoctorStatus.ActivePermanent.ToString(),
                    HourlyChargingRate = 700,
                    _id = doctorId4.ToString(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test E",
                    Specialism = DoctorSpecialism.Urology.ToString(),
                    Status = DoctorStatus.ActiveVisiting.ToString(),
                    HourlyChargingRate = 600,
                    _id = doctorId5.ToString(),
                },
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.Inactive.ToString(),
                    HourlyChargingRate = 750,
                    _id = doctorId6.ToString(),
                }
            });

            _repository = new DoctorsRepository(mongoFactory.Object, new Mock<ILogger>().Object);
        }

        #region Get Doctors By Query

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
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.ActivePermanent.ToString(),
                    HourlyChargingRate = 800,
                },
                new DoctorReadModel
                {
                    Name = "Dr Test B",
                    Specialism = DoctorSpecialism.Psychiatry.ToString(),
                    Status = DoctorStatus.ActiveVisiting.ToString(),
                    HourlyChargingRate = 500,
                },
                new DoctorReadModel
                {
                    Name = "Dr Test C",
                    Specialism = DoctorSpecialism.Neurology.ToString(),
                    Status = DoctorStatus.Inactive.ToString(),
                    HourlyChargingRate = 600,
                },
                new DoctorReadModel
                {
                    Name = "Mr Test D",
                    Specialism = DoctorSpecialism.Psychology.ToString(),
                    Status = DoctorStatus.ActivePermanent.ToString(),
                    HourlyChargingRate = 700,
                },
                new DoctorReadModel
                {
                    Name = "Dr Test E",
                    Specialism = DoctorSpecialism.Urology.ToString(),
                    Status = DoctorStatus.ActiveVisiting.ToString(),
                    HourlyChargingRate = 600,
                },
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.Inactive.ToString(),
                    HourlyChargingRate = 750,
                }
            }, options => options.Excluding(x => x._id).Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QuerySingleSpecialism_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                Specialisms = new List<string> { DoctorSpecialism.Orthopaedics.ToString() }
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(2);
            result.doctors.Should().BeEquivalentTo(new List<DoctorReadModel> {
                new DoctorReadModel
                {
                    Name = "Dr Test A",
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.ActivePermanent.ToString(),
                    HourlyChargingRate = 800,
                },
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.Inactive.ToString(),
                    HourlyChargingRate = 750,
                }
            }, options => options.Excluding(x => x._id).Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryMultipleSpecialisms_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                Specialisms = new List<string> { DoctorSpecialism.Orthopaedics.ToString(), DoctorSpecialism.Neurology.ToString() }
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
                Status = new List<string> { DoctorStatus.Inactive.ToString() }
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(2);
            result.doctors.Should().BeEquivalentTo(new List<DoctorReadModel> {
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.Inactive.ToString(),
                    HourlyChargingRate = 750,
                },
                new DoctorReadModel
                {
                    Name = "Dr Test C",
                    Specialism = DoctorSpecialism.Neurology.ToString(),
                    Status = DoctorStatus.Inactive.ToString(),
                    HourlyChargingRate = 600,
                }
            }, options => options.Excluding(x => x._id).Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryMultipleStatus_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                Status = new List<string> { DoctorStatus.Inactive.ToString(), DoctorStatus.ActivePermanent.ToString() }
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
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.ActivePermanent.ToString(),
                    HourlyChargingRate = 800,
                }
            }, options => options.Excluding(x => x._id).Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryDoctorId_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                DoctorId = new List<string>() { doctorId1.ToString() }
            };

            var result = await _repository.GetDoctors(q);

            result.Should().NotBeNull();
            result.doctors.Count().Should().Be(1);
            result.doctors.Should().BeEquivalentTo(new List<DoctorReadModel>()
            {
                new DoctorReadModel
                {
                    _id = doctorId1.ToString(),
                    Name = "Dr Test A",
                    Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                    Status = DoctorStatus.ActivePermanent.ToString(),
                    HourlyChargingRate = 800,
                }
            }, options => options.Excluding(x => x._id));
        }

        [Fact]
        public async Task WhenGetDoctors_QueryMultipleDoctorIds_ThenExpectedResult()
        {
            var q = new DoctorsQueryModel
            {
                DoctorId = new List<string>() { doctorId1.ToString(), doctorId2.ToString() }
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
                Specialism = DoctorSpecialism.Neurology.ToString(),
                Status = DoctorStatus.Inactive.ToString(),
                HourlyChargingRate = 600,
                _id = doctorId3.ToString(),
            }, options => options.Excluding(x => x._id));

            result.doctors[5].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test E",
                Specialism = DoctorSpecialism.Urology.ToString(),
                Status = DoctorStatus.ActiveVisiting.ToString(),
                HourlyChargingRate = 600,
                _id = doctorId5.ToString(),
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
                Specialism = DoctorSpecialism.Neurology.ToString(),
                Status = DoctorStatus.Inactive.ToString(),
                HourlyChargingRate = 600,
                _id = doctorId3.ToString(),
            }, options => options.Excluding(x => x._id));

            result.doctors[5].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Mr Test D",
                Specialism = DoctorSpecialism.Psychology.ToString(),
                Status = DoctorStatus.ActivePermanent.ToString(),
                HourlyChargingRate = 700,
                _id = doctorId4.ToString(),
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
                Specialism = DoctorSpecialism.Psychiatry.ToString(),
                Status = DoctorStatus.ActiveVisiting.ToString(),
                HourlyChargingRate = 500,
                _id = doctorId2.ToString(),
            }, options => options.Excluding(x => x._id));

            result.doctors[5].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test A",
                Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                Status = DoctorStatus.ActivePermanent.ToString(),
                HourlyChargingRate = 800,
                _id = doctorId1.ToString(),
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
                Specialism = DoctorSpecialism.Neurology.ToString(),
                Status = DoctorStatus.Inactive.ToString(),
                HourlyChargingRate = 600,
                _id = doctorId3.ToString(),
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
                Specialism = DoctorSpecialism.Psychology.ToString(),
                Status = DoctorStatus.ActivePermanent.ToString(),
                HourlyChargingRate = 700,
                _id = doctorId4.ToString(),
            }, options => options.Excluding(x => x._id));

            result.doctors[1].Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test E",
                Specialism = DoctorSpecialism.Urology.ToString(),
                Status = DoctorStatus.ActiveVisiting.ToString(),
                HourlyChargingRate = 600,
                _id = doctorId5.ToString(),
            }, options => options.Excluding(x => x._id));
        }

        #endregion

        #region Get Doctor By Id

        [Fact] 
        public async Task WhenGetDoctorById_Found_ThenExpectedResult()
        {
            var doctorId = doctorId1.ToString();
            var result = await _repository.GetDoctorById(doctorId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new DoctorReadModel
            {
                Name = "Dr Test A",
                Specialism = DoctorSpecialism.Orthopaedics.ToString(),
                Status = DoctorStatus.ActivePermanent.ToString(),
                HourlyChargingRate = 800,
                _id = doctorId,
            });
        }

        [Fact]
        public async Task WhenGetDoctorById_NotFound_ThenExpectedResult()
        {
            var doctorId = ObjectId.GenerateNewId().ToString();
            var result = await _repository.GetDoctorById(doctorId);

            result.Should().BeNull();
        }

        #endregion

        #region Post Doctor

        [Fact]
        public async Task WhenPostDoctor__ThenExpectedResult()
        {
            ObjectId id = ObjectId.GenerateNewId();
            DoctorReadModel q = new DoctorReadModel
            {
                _id = id.ToString(),
                Name = "test",
                HourlyChargingRate = 800,
                Status = DoctorStatus.Inactive.ToString(),
                Specialism = DoctorSpecialism.Orthopaedics.ToString()
            };

            await _repository.UpsertDoctor(q);

            var response = await _repository.GetDoctors(new DoctorsQueryModel { });

            response.Should().NotBeNull();
            response.doctors.Count().Should().Be(7);
            response.doctors[0].Should().BeEquivalentTo(q);
        }

        #endregion

        #region Delete Doctor



        #endregion
    }
}
