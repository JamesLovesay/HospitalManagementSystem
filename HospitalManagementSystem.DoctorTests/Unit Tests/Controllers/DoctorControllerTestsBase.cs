using HospitalManagementSystem.DoctorTests.Unit_Tests.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
//using Microsoft.AspNetCore.Mvc.Testing;

namespace HospitalManagementSystem.DoctorTests.Unit_Tests.Controllers
{
    public abstract class DoctorControllerTestsBase : IClassFixture<ApiWebApplicationFactory>
    {
        protected ApiWebApplicationFactory Factory;
        protected HttpClient Client;

        public DoctorControllerTestsBase(ApiWebApplicationFactory factory)
        {
            Factory = factory;
            Client = Factory.CreateClient();
            Client.Timeout = new TimeSpan(0, 1, 0);
        }
    }
}
