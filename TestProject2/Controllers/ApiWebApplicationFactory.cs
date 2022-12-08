using HospitalManagementSystem.Api.Controllers;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Serilog;

namespace HospitalManagementSystem.Api.Tests.Controllers
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly Mock<DoctorsRepository> _mockRepository;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IAuthorizationHandler> _mockAuthorizationHandler;
        private readonly Mock<ILogger> _mockLogger = new Mock<ILogger>();
        private readonly Mock<IMongoFactory> _mongoFactory;
        private readonly Mock<DoctorController> _mockController = new Mock<DoctorController>();

        public ApiWebApplicationFactory()
        {
            _mockMediator = new Mock<IMediator>();
            _mockRepository = new Mock<DoctorsRepository>();
            _mockAuthorizationHandler = new Mock<IAuthorizationHandler>();
            _mockLogger = new Mock<ILogger>();
            _mongoFactory = new Mock<IMongoFactory>();
            _mockController = new Mock<DoctorController>();
        }
        public Mock<DoctorsRepository> Repository => _mockRepository;
        public Mock<IMediator> Mediator => _mockMediator;
        public Mock<IAuthorizationHandler> AuthorisationHandler => _mockAuthorizationHandler;
        public Mock<ILogger> Logger => _mockLogger;
        public Mock<IMongoFactory> Factory => _mongoFactory;
        public Mock<DoctorController> Controller => _mockController;

    }
}