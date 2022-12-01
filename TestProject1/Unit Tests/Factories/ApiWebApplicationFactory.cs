using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DoctorTests.Unit_Tests.Factories
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly Mock<IMediator> _mockMediator;

        public ApiWebApplicationFactory()
        {
            _mockMediator = new Mock<IMediator>();
        }
    }
}
