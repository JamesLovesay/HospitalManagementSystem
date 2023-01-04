using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace HospitalManagementSystem.Api.Tests.Controllers
{
    public class ExampleAlternateControllerTestStructure
    {
        //HttpClient _client;

        //public ExampleAlternateControllerTestStructure()
        //{
        //    var server = new TestServer(new WebHostBuilder()
        //    .UseStartup<Startup>());
        //    _client = server.CreateClient();
        //}

        //[Fact]
        //public async Task GetDoctorsByQuery_ValidQueryReturnsNoResults_ThenExpectedResult()
        //{
        //    var doctorId = new ObjectId("565656565656565656565656");
        //    // Call the GET endpoint.
        //    var response = await _client.GetAsync($"/api/Doctor/query?doctorid={doctorId}");

        //    // Assert that the endpoint returns the expected status code.
        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        //    // Assert that the endpoint returns the expected data.
        //    var result = JsonConvert.DeserializeObject<DoctorsQueryResponse>(await response.Content.ReadAsStringAsync());
        //    Assert.Equal(null, result?.Doctors);
        //}

        //[Fact]
        //public async Task WhenGetDoctorsByQuery_InvalidPageLessThanOne_ThenExpectedResult()
        //{
        //    var response = await _client.GetAsync($"/api/Doctor/query?page=-1&pagesize=20");

        //    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        //}

    }
}
