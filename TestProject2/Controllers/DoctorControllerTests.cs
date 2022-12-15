using FluentAssertions;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json;
using System.Net;

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
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_InvalidPageSizeLessThanOne_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?page=1&pagesize=-1");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidPageSizeTooBig_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?page=1&pagesize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSortBy_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?sortby=notasortby");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
       }


        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSortDirection_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?sortdirection=notasortdirection");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
     }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSpecialism_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?specialism=notaspecialism");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidStatus_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctors/query?status=notastatus");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.SortDirection == "asc" && y.SortBy == "name"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned, Detail = new DoctorsQueryDetail { Page = 1, PageSize = 20, TotalPages = 0, TotalRecords = 0 } });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?sortdirection=asc&sortby=name");

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

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.SortDirection == "asc" && y.SortBy == "name"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidButNonExistentIdReturnsNoResults_ThenExpectedResult()
        {
            List<Doctor> returned = new List<Doctor>();
            var doctorId = new ObjectId("675845949586758586758587");
            string stringDoctorId = doctorId.ToString();

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId == new List<ObjectId> { doctorId }), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

            HttpResponseMessage response = await Client.GetAsync($"/api/Doctors/query?doctorid={stringDoctorId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new DoctorsQueryResponse
            {
                Doctors = new List<Doctor>(),
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId == new List<ObjectId> { doctorId }), It.IsAny<CancellationToken>()), Times.Once);
        }

        //[Fact]
        //public async Task WhenGetDoctorsByQuery_EmptyRequest_ThenExpectedResult()
        //{
        //    var returned = new List<Doctor>();

        //    Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId == null), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

        //    var response = await Client.GetAsync($"/api/Doctor/query");

        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        //    var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

        //    result.Should().BeEquivalentTo(new DoctorsQueryResponse
        //    {
        //        Doctors = new List<Doctor>(),
        //    });

        //    Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId == null), It.IsAny<CancellationToken>()), Times.Once);
        //}

        //[Fact]
        //public async Task WhenGetDoctorsByQuery_RequestHasData_ThenExpectedResult()
        //{
        //    var doctorId1 = new ObjectId("858473646577474734835748");
        //    var doctorId2 = new ObjectId("858473646577474734835749");

        //    var returned = new List<Doctor>
        //    {
        //        new Doctor
        //        {
        //            Name = "Dr Test A",
        //            Specialism = DoctorSpecialism.Orthopaedics,
        //            Status = DoctorStatus.ActivePermanent,
        //            HourlyChargingRate = 800,
        //            DoctorId = doctorId1,
        //        },
        //        new Doctor
        //        {
        //            Name = "Dr Test B",
        //            Specialism = DoctorSpecialism.Psychiatry,
        //            Status = DoctorStatus.ActiveVisiting,
        //            HourlyChargingRate = 500,
        //            DoctorId = doctorId2,
        //        }
        //    };

        //    Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => IsEquivalent(y.DoctorId, new List<ObjectId> { doctorId1, doctorId2 })), It.IsAny<CancellationToken>()))
        //   .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

        //    var response = await Client.GetAsync($"/api/Doctor/query?doctorid={doctorId1}&doctorid={doctorId2}");

        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

        //    var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());

        //    result.Should().BeEquivalentTo(new DoctorsQueryResponse
        //    {
        //        Doctors = returned
        //    });

        //    Factory.Mediator.Verify(x => x.Send(It.Is<DoctorsQuery>(y => IsEquivalent(y.DoctorId, new List<ObjectId> { doctorId1, doctorId2 })), It.IsAny<CancellationToken>()), Times.Once);


        //}

        #endregion
    }
}