using HospitalManagementSystem.Api.Controllers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using MediatR;
using Moq;
using Serilog;
using System.Net;

namespace HospitalManagementSystem.Api.Tests.Controllers
{
    public class DoctorsTests
    {
        private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
        private readonly Mock<ILogger> _logger = new Mock<ILogger>();
        private readonly Mock<DoctorsRepository> _sut = new Mock<DoctorsRepository>();
        private readonly Mock<IMongoFactory> _mongoFactory;
        private readonly Mock<DoctorController> _mockController = new Mock<DoctorController>();

        public DoctorsTests()
        {
            _sut = new Mock<DoctorsRepository>(_mongoFactory.Object, _logger.Object);
        }

        [Fact]
        public async Task GetDoctors_ShouldReturnDoctors()
        {
            Doctor test1 = new Doctor("Dr Test A", 800, DoctorSpecialism.Orthopaedics, DoctorStatus.ActivePermanent) { };

            _sut.Setup(s => s.GetDoctors(new DoctorsQueryModel())).ReturnsAsync(new List<DoctorReadModel>()
            {

            });

            //GetAsync($"/api/Doctors/doctors?page=0&pagesize=-1");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await

            result.Should().BeEquivalentTo($"The BroadcastJobQuery is invalid.{Environment.NewLine}1 error(s) found:{Environment.NewLine}[\"'Page Size' must be greater than '0'.\"]");
        }
    }
}