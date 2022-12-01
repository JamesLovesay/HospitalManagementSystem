using HospitalManagementSystem.Infra.MongoDBStructure.Config;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Serilog;

namespace HospitalManagementSystem.Infra.MongoDBStructure
{
    public class MongoFactory : IMongoFactory
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;

        public MongoFactory(MongoConfig config)
        {
            try
            {
                //alternate connection code from MongoDB Atlas

                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://JamesLovesay:J3WZdocNHuW6AQ0W@hospitalmanagementsyste.oqbsjn2.mongodb.net/?retryWrites=true&w=majority");
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                _client = new MongoClient(settings);
                _db = _client.GetDatabase("HospitalManagementSystem");

                //var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(ComposeConnectionString(config)));
                //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                //clientSettings.ServerSelectionTimeout = new TimeSpan(0, 0, 10);

                //_client = new MongoClient(clientSettings);
                //_client.WithWriteConcern(WriteConcern.WMajority);
                //_db = _client.GetDatabase(config.DbName);
            }
            catch (Exception ex)
            {
                Log.Error($"A MongoDB exception occurred. Exception={ex}");
                throw;
            }
        }

        private string ComposeConnectionString(MongoConfig mongoConfig)
        {
            var userPassword = !string.IsNullOrEmpty(mongoConfig.User) &&
                               !string.IsNullOrEmpty(mongoConfig.Password) ? $"{mongoConfig.User}:{mongoConfig.Password}@" : "";
            const string delimiter = "://";
            var arrConnString = mongoConfig.ConnectionString.Split(new[] { delimiter }, StringSplitOptions.None);
            return $"{arrConnString[0]}://{userPassword}{arrConnString[1]}";
        }


        public IMongoDatabase GetDatabase()
        {
            return _db;
        }

        public IMongoClient GetClient()
        {
            return _client;
        }
    }
}
