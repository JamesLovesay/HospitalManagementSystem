using FluentAssertions;
using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Controllers;
using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Patients;
using HospitalManagementSystem.Api.Validators.Patients;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Moq.Language.Flow;
using FluentValidation;
using MongoDB.Driver;

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
            var response = new PatientsQueryResponse { Patients = new List<Patient>() };

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

        #region Get Patient By Id

        [Fact]
        public async Task WhenGetPatientById_ValidRequest_ThenExpectedResult()
        {
            var patientId = ObjectId.GenerateNewId().ToString();

            var expectedResponse = new PatientRecordQueryResponse
            {
                PatientId = patientId
            };

            Factory.Mediator.Setup(x => x.Send(It.Is<PatientRecordQuery>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var response = await Client.GetAsync($"/api/Patients/{patientId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = JsonConvert.DeserializeObject<PatientRecordQueryResponse>(await response.Content.ReadAsStringAsync());

            result.Should().BeEquivalentTo(new PatientRecordQueryResponse
            {
                PatientId = patientId
            });

            Factory.Mediator.Verify(x => x.Send(It.Is<PatientRecordQuery>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetPatientById_InvalidObjectId_ThenBadRequest()
        {
            var patientId = "invalidObjectId";

            var response = await Client.GetAsync($"/api/Patients/{patientId}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain("PatientId is invalid. Please enter a valid object Id of length 24 characters.");

            Factory.Mediator.Verify(x => x.Send(It.IsAny<PatientRecordQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task WhenGetPatientById_NullResponse_ThenNotFound()
        {
            var patientId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.Is<PatientRecordQuery>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PatientRecordQueryResponse)null);

            var response = await Client.GetAsync($"/api/Patients/{patientId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            Factory.Mediator.Verify(x => x.Send(It.Is<PatientRecordQuery>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetPatientById_NotFound_ThenNotFound()
        {
            var patientId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.Is<PatientRecordQuery>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PatientRecordQueryResponse(true));

            var response = await Client.GetAsync($"/api/Patients/{patientId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            Factory.Mediator.Verify(x => x.Send(It.Is<PatientRecordQuery>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenGetPatientById_ExceptionThrown_ThenInternalServerError()
        {
            var patientId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.Is<PatientRecordQuery>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test Exception"));

            var response = await Client.GetAsync($"/api/Patients/{patientId}");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            Factory.Mediator.Verify(x => x.Send(It.Is<PatientRecordQuery>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Delete Patient

        [Fact]
        public async Task WhenDeletePatient_ValidPatientId_ReturnsNoContent()
        {
            // Arrange
            var patientId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.Is<DeletePatientCommand>(c => c.PatientId == patientId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var response = await Client.DeleteAsync($"/api/Patients/{patientId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            Factory.Mediator.Verify(x => x.Send(It.Is<DeletePatientCommand>(c => c.PatientId == patientId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenDeletePatient_InvalidPatientId_ReturnsBadRequest()
        {
            // Arrange
            var invalidPatientId = "invalid-patient-id";

            // Act
            var response = await Client.DeleteAsync($"/api/Patients/{invalidPatientId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenDeletePatient_PatientNotFound_ReturnsNotFound()
        {
            // Arrange
            var patientId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.Is<DeletePatientCommand>(c => c.PatientId == patientId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var response = await Client.DeleteAsync($"/api/Patients/{patientId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            Factory.Mediator.Verify(x => x.Send(It.Is<DeletePatientCommand>(c => c.PatientId == patientId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenDeletePatient_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var patientId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.Is<DeletePatientCommand>(c => c.PatientId == patientId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var response = await Client.DeleteAsync($"/api/Patients/{patientId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            Factory.Mediator.Verify(x => x.Send(It.Is<DeletePatientCommand>(c => c.PatientId == patientId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenDeletePatient_Unsuccessful_ThenNotFound()
        {
            var patientId = ObjectId.GenerateNewId().ToString();

            Factory.Mediator.Setup(x => x.Send(It.Is<DeletePatientCommand>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var response = await Client.DeleteAsync($"/api/Patients/{patientId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            Factory.Mediator.Verify(x => x.Send(It.Is<DeletePatientCommand>(y => y.PatientId == patientId), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Post Patient

        [Fact]
        public async Task WhenCreatePatientWithValidCommand_ThenPatientCreatedSuccessfully()
        {
            try
            {
                // Arrange
                var expectedPatientId = ObjectId.GenerateNewId();
                var command = new CreatePatientCommand
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                    Gender = "Male",
                    PhoneNumber = "5551234"
                };

                Factory.Mediator.Setup(x => x.Send(It.IsAny<CreatePatientCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedPatientId);

                // Act
                var response = await Client.PostAsJsonAsync("/api/Patients", command);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Created);

                var content = await response.Content.ReadAsStringAsync();
                content.Should().Contain("Patient created successfully");

                Factory.Mediator.Verify(x => x.Send(
                    It.IsAny<CreatePatientCommand>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            }
            finally
            {
                Factory.Mediator.Invocations.Clear();
            }
        }

        [Fact]
        public async Task WhenCreatePatientWithInvalidCommand_ThenBadRequest()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FirstName = "",
                LastName = "",
                DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                Gender = "",
                PhoneNumber = ""
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Patients", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenCreatePatientThrowsException_ThenInternalServerError()
        {
            try
            {
                // Arrange
                var command = new CreatePatientCommand
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                    Gender = "Male",
                    PhoneNumber = "07770551234"
                };

                Factory.Mediator.Setup(x => x.Send(It.IsAny<CreatePatientCommand>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new Exception());

                // Act
                var response = await Client.PostAsJsonAsync("/api/Patients", command);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

                Factory.Mediator.Verify(x => x.Send(It.IsAny<CreatePatientCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }
            finally
            {
                Factory.Mediator.Invocations.Clear();
            }
        }

        [Fact]
        public async Task WhenCreatePatient_InvalidData_ThenBadRequest()
        {
            try
            {
                var invalidCmd = new CreatePatientCommand
                {
                    FirstName = "",
                    LastName = "Doe",
                    DateOfBirth = DateTime.UtcNow.AddYears(-200).ToString(),
                    PhoneNumber = "1234567890"
                };

                var validator = new CreatePatientCommandValidator();
                var result = validator.Validate(invalidCmd);

                Factory.Mediator.Setup(x => x.Send(It.IsAny<CreatePatientCommand>(), It.IsAny<CancellationToken>()))
                    .Throws(new ValidationException(result.Errors));

                var response = await Client.PostAsJsonAsync("/api/Patients", invalidCmd);

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                var content = await response.Content.ReadAsStringAsync();
                content.Should().Contain(result.Errors.First().ErrorMessage);

                Factory.Mediator.Verify(x => x.Send(It.IsAny<CreatePatientCommand>(), It.IsAny<CancellationToken>()), Times.Never);

            }
            finally
            {
                Factory.Mediator.Invocations.Clear();
            }
        }

        #endregion

        #region Put Patient

        [Fact]
        public async Task GivenValidPatientIdAndCommand_WhenUpdatePatient_ThenReturnOkResult()
        {
            // Arrange
            var patientId = ObjectId.GenerateNewId().ToString();
            var command = new UpdatePatientCommand
            {
                PatientId = patientId,
                DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                Gender = "Male",
                PhoneNumber = "5551234"
            };
            Factory.Mediator.Setup(x => x.Send(It.IsAny<UpdatePatientCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var response = await Client.PutAsJsonAsync($"/api/Patients/{patientId}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain($"Update command for patient issued successfully. PatientId={patientId}");

            Factory.Mediator.Verify(x => x.Send(It.IsAny<UpdatePatientCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GivenInvalidPatientId_WhenUpdatePatient_ThenReturnBadRequestResult()
        {
            // Arrange
            var invalidPatientId = "invalid_id";
            var command = new UpdatePatientCommand
            {
                PatientId = invalidPatientId,
                DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                Gender = "Male",
                PhoneNumber = "5551234"
            };

            // Act
            var response = await Client.PutAsJsonAsync($"/api/Patients/{invalidPatientId}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenMismatchedPatientIdInCommand_WhenUpdatePatient_ThenReturnBadRequestResult()
        {
            // Arrange
            var patientId = ObjectId.GenerateNewId().ToString();
            var command = new UpdatePatientCommand
            {
                PatientId = ObjectId.GenerateNewId().ToString(),
                DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                Gender = "Male",
                PhoneNumber = "5551234"
            };

            // Act
            var response = await Client.PutAsJsonAsync($"/api/Patients/{patientId}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenInvalidCommand_WhenUpdatePatient_ThenReturnBadRequestResult()
        {
            // Arrange
            var patientId = ObjectId.GenerateNewId().ToString();
            var command = new UpdatePatientCommand
            {
                PatientId = patientId,
                DateOfBirth = "Invalid Date",
                Gender = "Male",
                PhoneNumber = "5551234"
            };

            // Act
            var response = await Client.PutAsync($"/api/Patients/{patientId}", GetHttpContent(command));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenPatientNotFound_WhenUpdatePatient_ThenReturnNotFoundResult()
        {
            // Arrange
            var patientId = ObjectId.GenerateNewId().ToString();
            var command = new UpdatePatientCommand
            {
                PatientId = patientId,
                DateOfBirth = new DateTime(1990, 01, 01).ToString(),
                Gender = "Male",
                PhoneNumber = "5551234"
            };
            Factory.Mediator.Setup(x => x.Send(It.IsAny<UpdatePatientCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var response = await Client.PutAsync($"/api/Patients/{patientId}", GetHttpContent(command));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain($"Patient not found for Id {patientId}");

            Factory.Mediator.Verify(x => x.Send(It.IsAny<UpdatePatientCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            #endregion
        }
    }
}