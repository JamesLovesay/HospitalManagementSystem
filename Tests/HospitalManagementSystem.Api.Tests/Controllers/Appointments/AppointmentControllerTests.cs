using FluentAssertions;
using HospitalManagementSystem.Api.Models.Appointments;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Queries.Appointments;
using HospitalManagementSystem.Api.Queries.Patients;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace HospitalManagementSystem.Api.Tests.Controllers.Appointments;

public class AppointmentControllerTests : AppointmentControllerTestsBase
{
    public AppointmentControllerTests(ApiWebApplicationFactory factory) : base(factory) { }

    #region Get Appointment By Id

    [Fact]
    public async Task GetAppointmentById_ReturnsOk_WhenAppointmentExists()
    {
        try
        {
            // Arrange
            var appointmentId = 1234;
            var returnedAppt = new AppointmentReadModel(appointmentId, "2023-04-12T16:30:00") { };

            Factory.Mediator.Setup(x => x.Send(It.Is<AppointmentRecordQuery>(x => x.Id == appointmentId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AppointmentRecordQueryResponse { Appointment = returnedAppt });

            // Act
            var response = await Client.GetAsync($"/api/Appointments/{appointmentId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AppointmentRecordQueryResponse>(responseContent);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new AppointmentRecordQueryResponse { Appointment = returnedAppt });

            Factory.Mediator.Verify(x => x.Send(It.IsAny<AppointmentRecordQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        finally
        {
            Factory.Mediator.Invocations.Clear();
        }
    }

    [Fact]
    public async Task GetAppointmentById_ReturnsNotFound_WhenAppointmentDoesNotExist()
    {
        // Act
        var response = await Client.GetAsync($"/api/Appointments/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenGetAppointmentById_InvalidId_ThenExpectedResult()
    {
        // Arrange
        var invalidAppointmentId = "invalid_id";

        // Act
        var response = await Client.GetAsync($"/api/Appointments/{invalidAppointmentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task WhenGetAppointmentById_ServerError_ThenExpectedResult()
    {
        try
        {
            // Arrange
            var appointmentId = 1;
            Factory.Mediator.Setup(x => x.Send(It.IsAny<AppointmentRecordQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var response = await Client.GetAsync($"/api/Appointments/{appointmentId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
        finally
        {
            Factory.Mediator.Invocations.Clear();
        }
    }

    #endregion
}
