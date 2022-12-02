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

            MongoClient client = new MongoClient(runner.ConnectionString);
            var database = client.GetDatabase("DoctorRepositoryTests");
            mongoFactory.Setup(x => x.GetDatabase()).Returns(database);

            _doctorCollection = database.GetCollection<DoctorReadModel>(nameof(DoctorReadModel));
            _doctorCollection.InsertMany(new List<DoctorReadModel> {
                new DoctorReadModel
                {
                    Name = "Dr Test A",
                    Specialism = "Orthopaedics",
                    Status = "ActivePermanent",
                    HourlyChargingRate = 800,
                    DoctorId = new MongoDB.Bson.ObjectId(),
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test B",
                    Specialism = "Psychiatry",
                    Status = "ActiveTemporary",
                    HourlyChargingRate = 500,
                    DoctorId = new MongoDB.Bson.ObjectId(),
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test C",
                    Specialism = "Neurology",
                    Status = "Inactive",
                    HourlyChargingRate = 600,
                    DoctorId = new MongoDB.Bson.ObjectId(),
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Mr Test D",
                    Specialism = "Psychology",
                    Status = "ActivePermanent",
                    HourlyChargingRate = 700,
                    DoctorId = new MongoDB.Bson.ObjectId(),
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Dr Test E",
                    Specialism = "Urology",
                    Status = "ActiveTemporary",
                    HourlyChargingRate = 600,
                    DoctorId = new MongoDB.Bson.ObjectId(),
                    _id = new MongoDB.Bson.ObjectId(),
                },
                new DoctorReadModel
                {
                    Name = "Mr Test F",
                    Specialism = "Orthopaedics",
                    Status = "Inactive",
                    HourlyChargingRate = 750,
                    DoctorId = new MongoDB.Bson.ObjectId(),
                    _id = new MongoDB.Bson.ObjectId(),
                }
            });
        }
    }
}
