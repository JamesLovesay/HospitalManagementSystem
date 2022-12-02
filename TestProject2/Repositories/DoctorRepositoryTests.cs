using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Api.Tests.Repositories
{
    public class DoctorRepositoryTests
    {
        private readonly IMongoCollection<DoctorReadModel>? _doctorCollection;
        private readonly DoctorsRepository? _repository;

        public DoctorRepositoryTests()
        {
            var mongoFactory = new Mock<IMongoFactory>();
            var runner = MongoDbRunner.Start();
        }
    }
}
