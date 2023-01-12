using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using HospitalManagementSystem.Infra.MongoDBStructure.ReadModels;
using MongoDB.Driver;
using Moq;
using Serilog;

namespace HospitalManagementSystem.Infra.MongoDBStructure.Tests
{
    public class TestReadModel : BaseReadModel
    {
        public string Value { get; set; }
        public override bool IsEnabled() => true;
    }
    public class ReadStoreTests
    {
        private readonly ReadStore _readStore;
        private readonly Mock<IMongoFactory> _mongoFactoryMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<BaseReadModel> _collection;

        public ReadStoreTests()
        {
            _mongoFactoryMock = new Mock<IMongoFactory>();
            _loggerMock = new Mock<ILogger>();
            _db = new MongoClient().GetDatabase("TestDB");
            _collection = _db.GetCollection<BaseReadModel>("BaseReadModel");
            _mongoFactoryMock.Setup(x => x.GetDatabase()).Returns(_db);

            _readStore = new ReadStore(_mongoFactoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task TestGetModelAsync()
        {
            var model = new TestReadModel { Id = Guid.NewGuid() };
            await _collection.InsertOneAsync(model);

            var result = await _readStore.GetModelAsync<TestReadModel>(model.Id);

            Assert.Equal(model.Id, result.Id);
            Assert.Equal(model.Created.Date, result.Created.Date);
        }

        [Fact]
        public async Task TestGetModelNoDefaultAsync()
        {
            var model = new TestReadModel { Id = Guid.NewGuid() };
            await _collection.InsertOneAsync(model);

            var result = await _readStore.GetModelNoDefaultAsync<TestReadModel>(model.Id);

            if (result != null)
            {
                Assert.Equal(model.Id, result.Id);
                Assert.Equal(model.Created.Date, result.Created.Date);
            }
            else
            {
                Assert.Equal(result, null);
            }
        }

        [Fact]
        public async Task TestSaveModelAsync()
        {
            var model = new TestReadModel { Id = Guid.NewGuid() };

            await _readStore.SaveModelAsync(model);
            await _collection.InsertOneAsync(model);

            var result = await _collection.FindAsync(x => x.Id == model.Id);
            var savedModel = result.FirstOrDefault();

            Assert.Equal(model.Id, savedModel.Id);
            Assert.Equal(model.Created.Date, savedModel.Created.Date);
            Assert.Equal(model.LastModified.Date, savedModel.LastModified.Date);
            Assert.Equal(1, savedModel.Version);
        }

        [Fact]
        public async Task TestSaveModelAsync_WithExistingModel()
        {
            var model = new TestReadModel { Id = Guid.NewGuid(), Version = 1};
            model.LastModified = DateTime.UtcNow;
            await _collection.InsertOneAsync(model);

            await _readStore.SaveModelAsync(model);

            var result = await _collection.FindAsync(x => x.Id == model.Id);
            var savedModel = result.FirstOrDefault();

            Assert.Equal(model.Id, savedModel.Id);
            Assert.Equal(model.Created.Date, savedModel.Created.Date);
            Assert.Equal(model.LastModified.Date, savedModel.LastModified.Date);
        }
    }
}
