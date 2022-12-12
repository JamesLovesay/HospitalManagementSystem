using FluentAssertions;
using HospitalManagementSystem.Api.Controllers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json;
using Serilog;
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
            var response = await Client.GetAsync($"/api/Doctor/query?page=-1&pagesize=20");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenGetDoctorsByQuery_InvalidPageSizeLessThanOne_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctor/query?page=1&pagesize=-1");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidPageSizeTooBig_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctor/query?page=1&pagesize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSortBy_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctor/query?sortby=notasortby");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
       }


        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSortDirection_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctor/query?sortdirection=notasortdirection");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
     }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidSpecialism_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctor/query?specialism=notaspecialism");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

        [Fact]
        public async Task WhenDoctorsByQuery_InvalidStatus_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Doctor/query?status=notastatus");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
       }

        #endregion

        #region Get Doctors Query Valid

        [Fact]
        public async Task WhenGetDoctorsByQuery_ValidButNonExistentIdReturnsNoResults_ThenExpectedResult()
        {
            var returned = new List<Doctor>();
            var doctorId = new ObjectId("675845949586758586758587");

            Factory.Mediator.Setup(x => x.Send(It.Is<DoctorsQuery>(y => y.DoctorId == new List<ObjectId> { doctorId }), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorsQueryResponse { Doctors = returned });

            var response = await Client.GetAsync($"/api/Doctor/query?doctorid={doctorId}");

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

        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

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