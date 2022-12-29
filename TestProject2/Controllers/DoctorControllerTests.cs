using FluentAssertions;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace HospitalManagementSystem.Api.Tests.Controllers
{
    public class DoctorControllerTests : DoctorControllerTestBase
    {
        public DoctorControllerTests(ApiWebApplicationFactory factory) : base(factory) { }

        #region Get Doctors Query Invalid

        [Fact]
        public async Task WhenGetDoctorsByQuery_InvalidPageLessThanOne_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?page=-1&pagesize=20");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Page must be greater than 0");
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_InvalidPageSizeLessThanOne_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?page=1&pagesize=-1");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Page Size must be greater than 0");
        }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidPageSizeTooBig_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?page=1&pagesize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Page Size must be less than 100");
        }


        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSortBy_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?sortby=notasortby");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"You can only sort on fields Name, Specialism, Status or HourlyChargingRate");
        }


        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSortDirection_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?sortdirection=notasortdirection");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"You can only sort by directions asc or desc");
        }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSpecialism_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?specialism=notaspecialism");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Specialism value(s) supplied were invalid");
        }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidStatus_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?status=notastatus");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Status value(s) supplied were invalid");
        }

        #endregion

        #region Get Doctors Query Valid

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidButNonExistentNameReturnsNoResults_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();
            
            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorName == "not a name"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?doctorname=not a name");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorName == "not a name"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidPageSize_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.PageSize == 3), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned, Detail = new DoctorsQueryDetail { Page = 1, PageSize = 3, TotalPages = 0, TotalRecords = 0 } });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?pagesize=3");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
                Detail = new DoctorsQueryDetail 
                { 
                    Page = 1, 
                    PageSize = 3, 
                    TotalPages = 0, 
                    TotalRecords = 0 
                }
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.PageSize == 3), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidPage_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.Page == 2), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned, Detail = new DoctorsQueryDetail { Page = 2, PageSize = 20, TotalPages = 0, TotalRecords = 0 } });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?page=2");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
                Detail = new DoctorsQueryDetail
                {
                    Page = 2,
                    PageSize = 20,
                    TotalPages = 0,
                    TotalRecords = 0
                }
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.Page == 2), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidSortBy_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.SortBy == "Name"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned, Detail = new DoctorsQueryDetail { Page = 1, PageSize = 20, TotalPages = 0, TotalRecords = 0 } });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?sortby=Name");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
                Detail = new DoctorsQueryDetail
                {
                    Page = 1,
                    PageSize = 20,
                    TotalPages = 0,
                    TotalRecords = 0
                }
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.SortBy == "Name"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidSortDirection_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.SortDirection == "asc"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned, Detail = new DoctorsQueryDetail { Page = 1, PageSize = 20, TotalPages = 0, TotalRecords = 0 } });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?sortdirection=asc");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
                Detail = new DoctorsQueryDetail
                {
                    Page = 1,
                    PageSize = 20,
                    TotalPages = 0,
                    TotalRecords = 0
                }
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.SortDirection == "asc"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_MultipleValidQueries_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.SortDirection == "desc" && y.SortBy == "name"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned, Detail = new DoctorsQueryDetail { Page = 1, PageSize = 20, TotalPages = 0, TotalRecords = 0 } });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?sortdirection=desc&sortby=name");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
                Detail = new DoctorsQueryDetail
                {
                    Page = 1,
                    PageSize = 20,
                    TotalPages = 0,
                    TotalRecords = 0
                }
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.SortDirection == "desc" && y.SortBy == "name"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidButNonExistentStatusReturnsNoResults_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.Status.Contains("inactive")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?status=inactive");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.Status.Contains("inactive")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidButNonExistentSpecialismReturnsNoResults_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.Specialism.Contains("generalsurgery")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?specialism=generalsurgery");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.Specialism.Contains("generalsurgery")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidButNonExistentIdReturnsNoResults_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();
            string doctorId = "264549568978495615485948";

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId.Contains(doctorId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?doctorid={doctorId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId.Contains(doctorId)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_EmptyRequest_ThenExpectedResult()
        {
            var returned = new List<Doctor>();
            var queryDetail = new DoctorsQueryDetail
            {
                Page = 1,
                PageSize = 10,
                TotalPages = 0,
                TotalRecords = 0,
            };

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId == null), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned, Detail = queryDetail });

            var response = await Client.GetAsync($"/api/Doctors/query");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
                Detail = queryDetail
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId == null), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_RequestHasData_ThenExpectedResult()
        {
            var doctorId1 = new ObjectId("858473646577474734835748");

            var returned = new List<Doctor>
            {
                new Doctor
                {
                    Name = "Dr Test A",
                    Specialism = DoctorSpecialism.Orthopaedics,
                    Status = DoctorStatus.ActivePermanent,
                    HourlyChargingRate = 800,
                    DoctorId = doctorId1.ToString(),
                },
            };

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => IsEquivalent(y.DoctorId, new List<string> { doctorId1.ToString() })), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

            var response = await Client.GetAsync($"/api/Doctors/query?doctorid={doctorId1}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = returned
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => IsEquivalent(y.DoctorId, new List<string> { doctorId1.ToString() })), It.IsAny<CancellationToken>()), Times.Once);


        }

        #endregion

        #region Post Doctor Invalid

        [Fact]
        public async Task WhenPostDoctor_InvalidName_ThenExpectedResult()
        {
            CreateDoctorCommand newDoctor = new CreateDoctorCommand
            {
                Name = "",
                HourlyChargingRate = 800,
                Status = "Inactive",
                Specialism = "Orthopaedics"
            };
            var response = await Client.PostAsync($"/api/Doctors", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenPostDoctor_InvalidRate_ThenExpectedResult()
        {
            CreateDoctorCommand newDoctor = new CreateDoctorCommand
            {
                Name = "Test",
                HourlyChargingRate = -1,
                Status = "Inactive",
                Specialism = "Orthopaedics"
            };
            var response = await Client.PostAsync($"/api/Doctors", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenPostDoctor_InvalidStatus_ThenExpectedResult()
        {
            CreateDoctorCommand newDoctor = new CreateDoctorCommand
            {
                Name = "Test",
                HourlyChargingRate = 800,
                Status = "Invalid",
                Specialism = "Orthopaedics"
            };
            var response = await Client.PostAsync($"/api/Doctors", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenPostDoctor_InvalidSpecialism_ThenExpectedResult()
        {
            CreateDoctorCommand newDoctor = new CreateDoctorCommand
            {
                Name = "Test",
                HourlyChargingRate = 800,
                Status = "Inactive",
                Specialism = "Invalid"
            };
            var response = await Client.PostAsync($"/api/Doctors", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Post Doctor Valid

        [Fact]
        public async Task WhenPostDoctor_Valid_ThenExpectedResult()
        {
            CreateDoctorCommand newDoctor = new CreateDoctorCommand 
            { 
                Name = "Test", 
                HourlyChargingRate = 800, 
                Status = "Inactive", 
                Specialism = "Orthopaedics" 
            };
            var response = await Client.PostAsync($"/api/Doctors", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        #endregion

        #region Get Doctor By ID

        [Fact]
        public async Task WhenGetNonExistentDoctor_ThenExpectedResult()
        {

            var doctorId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.IsAny<DoctorRecordQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorRecordQueryResponse(true));

            var response = await Client.GetAsync($"/api/Doctors/{doctorId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task WhenGetDoctorNotObjectId_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/notanobjectid");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"DoctorId is invalid. Please enter a valid object Id of length 24 characters.");
        }

        [Fact]
        public async Task WhenGetValidDoctor_ThenExpectedResult()
        {
            var doctorId = ObjectId.GenerateNewId().ToString();

            var expectedResponse = new DoctorRecordQueryResponse
            {
                DoctorId = doctorId
            };

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorRecordQuery>(y => y.DoctorId == doctorId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var response = await Client.GetAsync($"/api/Doctors/{doctorId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = JsonConvert.DeserializeObject<DoctorRecordQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorRecordQueryResponse
            {
                DoctorId = doctorId
            });
        }
        #endregion

        #region Delete Doctor

        [Fact]
        public async Task WhenDeleteDoctor_InvalidId_ThenExpectedResult()
        {
            var doctorId = "invalid ID";

            Factory.Mediator.Setup(x => x.Send(It.IsAny<DoctorDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var response = await Client.DeleteAsync($"/api/Doctors/{doctorId}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Doctor Id is invalid");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task WhenDeleteDoctor_NoIdProvided_ThenExpectedResult(string id)
        {
            Factory.Mediator.Setup(x => x.Send(It.IsAny<DoctorDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var response = await Client.DeleteAsync($"/api/Doctors/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task WhenDeleteDoctor_ValidId_ThenExpectedResult()
        {
            string doctorId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.IsAny<DoctorDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var response = await Client.DeleteAsync($"/api/Doctors/{doctorId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Delete command for doctor issued successfully. DoctorId={doctorId}");
        }

        #endregion

        #region Put Doctor


        [Fact]
        public async Task WhenPutDoctor_InvalidName_ThenBadRequestExpectedResult()
        {
            string id = ObjectId.GenerateNewId().ToString();

            DoctorUpdateCommand newDoctor = new DoctorUpdateCommand
            {
                Name = "",
                HourlyChargingRate = 800,
                Status = "Inactive",
                DoctorId = id,
            };
            var response = await Client.PutAsync($"/api/Doctors/{id}", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task WhenPutDoctor_InvalidRate_ThenBadRequestExpectedResult()
        {
            string id = ObjectId.GenerateNewId().ToString();

            DoctorUpdateCommand newDoctor = new DoctorUpdateCommand
            {
                Name = "test name",
                HourlyChargingRate = -800,
                Status = "Inactive",
                DoctorId = id,
            };
            var response = await Client.PutAsync($"/api/Doctors/{id}", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Rate must be greater than 0");
        }

        [Fact]
        public async Task WhenPutDoctor_InvalidStatus_ThenBadRequestExpectedResult()
        {
            string id = ObjectId.GenerateNewId().ToString();

            DoctorUpdateCommand newDoctor = new DoctorUpdateCommand
            {
                Name = "test name",
                HourlyChargingRate = 800,
                Status = "Invalid",
                DoctorId = id,
            };
            var response = await Client.PutAsync($"/api/Doctors/{id}", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Status value(s) supplied were invalid: Invalid");
        }

        [Fact]
        public async Task WhenPutDoctor_InvalidId_ThenBadRequestExpectedResult()
        {
            string id = "not an id";

            DoctorUpdateCommand newDoctor = new DoctorUpdateCommand
            {
                Name = "test name",
                HourlyChargingRate = 800,
                Status = "Inactive",
                DoctorId = id,
            };

            var response = await Client.PutAsync($"/api/Doctors/{id}", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Please enter a valid DoctorId");
        }

        [Fact]
        public async Task WhenPutDoctor_ValidButNonExistentId_ThenNotFoundExpectedResult()
        {
            string id = ObjectId.GenerateNewId().ToString();

            DoctorUpdateCommand newDoctor = new DoctorUpdateCommand
            {
                Name = "test name",
                HourlyChargingRate = 800,
                Status = "Inactive",
                DoctorId = id,
            };

            var response = await Client.PutAsync($"/api/Doctors/{id}", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Doctor not found for Id {id}");
        }

        [Fact]
        public async Task WhenPutDoctor_Valid_ThenOkExpectedResult()
        {
            string id = ObjectId.GenerateNewId().ToString();

            DoctorUpdateCommand newDoctor = new DoctorUpdateCommand
            {
                Name = "test name",
                HourlyChargingRate = 800,
                Status = "Inactive",
                DoctorId = id,
            };

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorUpdateCommand>(y => y.DoctorId == id), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

            var response = await Client.PutAsync($"/api/Doctors/{id}", GetHttpContent(newDoctor));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadAsStringAsync();
        }


        #endregion
    }
}