using FluentValidation;
using HospitalManagementSystem.Api.Controllers;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Api.Validators;
using HospitalManagementSystem.Infra.MongoDBStructure.Config;
using HospitalManagementSystem.Infra.MongoDBStructure;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace HospitalManagementSystem.Api.Tests.Controllers
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly Mock<IDoctorsRepository> _mockRepository;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger> _mockLogger = new Mock<ILogger>();
        private readonly Mock<IMongoFactory> _mongoFactory;

        public ApiWebApplicationFactory()
        {
            _mockMediator = new Mock<IMediator>();
            _mockRepository = new Mock<IDoctorsRepository>();
            _mockLogger = new Mock<ILogger>();
            _mongoFactory = new Mock<IMongoFactory>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                //services.AddSingleton(new Mock<ILogger>().Object);
                services.AddSingleton(_mockMediator.Object);
                services.AddSingleton(_mockRepository.Object);

            });
        }
        public Mock<IDoctorsRepository> Repository => _mockRepository;
        public Mock<IMediator> Mediator => _mockMediator;
        public Mock<ILogger> Logger => _mockLogger;
        public Mock<IMongoFactory> Factory => _mongoFactory;
    }
}