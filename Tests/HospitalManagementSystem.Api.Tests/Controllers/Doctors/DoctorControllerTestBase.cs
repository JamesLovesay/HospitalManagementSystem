using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace HospitalManagementSystem.Api.Tests.Controllers.Doctors
{
    public class DoctorControllerTestBase : IClassFixture<ApiWebApplicationFactory>
    {
        protected ApiWebApplicationFactory Factory;
        protected HttpClient Client;

        public DoctorControllerTestBase(ApiWebApplicationFactory factory)
        {
            Factory = factory;
            Client = Factory.CreateClient();
            Client.Timeout = new TimeSpan(0, 1, 0);
        }

        protected HttpContent GetHttpContent(object content) =>
            new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        protected string SanitiseLineEnds(string s)
        {
            return s.Replace("\r", "").Replace("\n", Environment.NewLine);
        }

        public bool IsEquivalent<T>(List<T> listA, List<T> listB)
        {
            try
            {
                listA.Should().BeEquivalentTo(listB);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
