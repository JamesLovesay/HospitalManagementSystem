using HospitalManagementSystem.Infra.MongoDBStructure.Config;

namespace HospitalManagementSystem.Infra.MongoDBStructure.Tests
{
    public class MongoFactoryTests
    {
        private readonly MongoConfig _config;
        private readonly MongoFactory _mongoFactory;

        public MongoFactoryTests()
        {
            _config = new MongoConfig
            {
                ConnectionString = "mongodb+srv://JamesLovesay:jQ70LRjw4SFnsFs3@hospitalmanagementsyste.oqbsjn2.mongodb.net/?retryWrites=true&w=majority",
                DbName = "HospitalManagementSystem"
            };

            _mongoFactory = new MongoFactory(_config);
        }

        [Fact]
        public void TestGetDatabase()
        {
            var db = _mongoFactory.GetDatabase();
            Assert.NotNull(db);
        }

        [Fact]
        public void TestGetClient()
        {
            var client = _mongoFactory.GetClient();
            Assert.NotNull(client);
        }
    }
}