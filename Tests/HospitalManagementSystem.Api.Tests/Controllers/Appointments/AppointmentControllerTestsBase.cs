using Newtonsoft.Json;
using System.Text;

namespace HospitalManagementSystem.Api.Tests.Controllers.Appointments;

public class AppointmentControllerTestsBase : IClassFixture<ApiWebApplicationFactory>
{
    protected ApiWebApplicationFactory Factory;
    protected HttpClient Client;

    public AppointmentControllerTestsBase(ApiWebApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
        Client.Timeout = new TimeSpan(0, 1, 0);
    }

    protected HttpContent GetHttpContent(object content) =>
        new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
}
