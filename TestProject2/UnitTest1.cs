using System.Net;
using HospitalManagementSystem.Api.Controllers;
using MediatR;
using Moq;
using Serilog;

namespace TestProject2
{
    public class UnitTest1
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger> _logger;

        [Fact]
        public async Task Test1()
        {
            //Arrange 

            //Act

            var doctorController = new Mock<DoctorController>(_mockMediator, _logger);
            var actionResult = await doctorController.Object;

            //Assert
            var response = await Client.GetAsync($"/api/Doctors/doctors?page=0&pagesize=-1");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().BeEquivalentTo($"The BroadcastJobQuery is invalid.{Environment.NewLine}1 error(s) found:{Environment.NewLine}[\"'Page Size' must be greater than '0'.\"]");
        }
    }
}