using FluentAssertions;
using FluentValidation.Results;
using FluentValidation;
using HospitalManagementSystem.Api.Controllers;
using HospitalManagementSystem.Api.Queries.Patients;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using HospitalManagementSystem.Api.Helpers;
using System.Net;
using HospitalManagementSystem.Api.Models.Patients;

namespace HospitalManagementSystem.Api.Tests.Controllers.Patients
{
    public class PatientControllerTests : PatientControllerTestsBase
    {
        public PatientControllerTests(ApiWebApplicationFactory factory) : base(factory) { }

        #region GetPatients Invalid

        [Fact]
        public async Task GetPatients_WithNullQuery_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mediator = new Mock<IMediator>();
            var logger = new Mock<ILogger<PatientsController>>();
            var controller = new PatientsController(mediator.Object, logger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.GetPatients(null));
        }

        [Fact]
        public async Task GetPatients_WithEmptyResponse_ShouldReturnNotFoundObjectResult()
        {
            // Arrange
            var query = new PatientsQuery { /* valid query parameters */ };
            var response = new PatientsQueryResponse { Patients = new List<Api.Models.Patients.Patient>() };

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(response);

            var logger = new Mock<ILogger<PatientsController>>();
            var controller = new PatientsController(mediator.Object, logger.Object);

            // Act
            var result = await controller.GetPatients(query);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var resultValue = Assert.IsType<PatientsQueryResponse>(notFoundResult.Value);
            Assert.Equal(response, resultValue);
        }

        [Fact]
        public async Task GetPatients_WithUnhandledException_ShouldReturnInternalServerErrorObjectResult()
        {
            // Arrange
            var query = new PatientsQuery { };
            var exception = new Exception("Unhandled exception occurred");

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(query, CancellationToken.None)).Throws(exception);

            var logger = new Mock<ILogger<PatientsController>>();
            var controller = new PatientsController(mediator.Object, logger.Object);

            // Act
            var result = await controller.GetPatients(query);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidPage_ThenBadRequest()
        {
            var response = await Client.GetAsync($"/api/Patients/query?page=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain("Page must be greater than 0");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidPageSize_ThenBadRequest()
        {
            var response = await Client.GetAsync($"/api/Patients/query?pagesize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Page Size must be greater than 0");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidSortBy_ThenBadRequest()
        {
            var response = await Client.GetAsync($"/api/Patients/query?sortby=notasortby");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"You can only sort on fields FirstName, LastName, DateOfBirth or AdmissionDate");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidSortDirection_ThenBadRequest()
        {
            var response = await Client.GetAsync($"/api/Patients/query?sortdirection=notadirection");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"You can only sort by directions asc or desc");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidGender_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Patients/query?gender=notagender");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Gender can only be Male, Female, or NonBinary");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidAdmissionDate_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Patients/query?admissiondate=notadate");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"AdmissionDate must match the format {QueryHelper.DateTimeFormat}");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidStatus_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Patients/query?status=notastatus");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Status value supplied was invalid. Can only be Discharged, Admitted or InTreatment");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidDateOfBirth_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Patients/query?dateofbirth=notadate");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"DateOfBirth must match the format {QueryHelper.DateTimeFormat}");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidSortDirection_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Patients/query?sortdirection=notadirection");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"You can only sort by directions asc or desc");
        }

        [Fact]
        public async Task WhenPatientsByQuery_InvalidPageSize_ThenExpectedResult()
        {
            var response = await Client.GetAsync($"/api/Patients/query?pagesize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain($"Page Size must be greater than 0");
        }

        #endregion

        #region GetPatients Valid

        [Fact]
        public async Task GetPatients_WithValidQuery_ShouldReturnOkObjectResult()
        {
            // Arrange
            var query = new PatientsQuery { PatientName = "Mr Test", Gender = "Male", isAdmitted = true, Page = 1, PageSize = 10, DateOfBirth = "1985-08-31", SortDirection = "ASC", AdmissionDate = "2023-01-01" };
            var response = new PatientsQueryResponse { Patients = new List<Patient>() { new Patient { PatientId = "4r5t6t67576758485746ryth45" } } };

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(response);

            var logger = new Mock<ILogger<PatientsController>>();
            var controller = new PatientsController(mediator.Object, logger.Object);

            // Act
            var result = await controller.GetPatients(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<PatientsQueryResponse>(okResult.Value);
            Assert.Equal(response, resultValue);
        }

        #endregion
    }
}