﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Api.Tests.Controllers.Patients
{
    public class PatientControllerTestsBase : IClassFixture<ApiWebApplicationFactory>
    {
        protected ApiWebApplicationFactory Factory;
        protected HttpClient Client;

        public PatientControllerTestsBase(ApiWebApplicationFactory factory)
        {
            Factory = factory;
            Client = Factory.CreateClient();
            Client.Timeout = new TimeSpan(0, 1, 0);
        }
    }
}
