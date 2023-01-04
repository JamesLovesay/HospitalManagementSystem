using MongoDB.Driver;
using Serilog;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;

namespace HospitalManagementSystem.Infra.MongoDBStructure
{
    public class ReadStore : IReadStore
    {
        protected readonly IMongoDatabase _db;
        protected readonly ILogger _logger;

        public ReadStore(IMongoFactory mongoFactory, ILogger logger)
        {
            _logger = logger;
            _db = mongoFactory.GetDatabase();
        }

        public async Task<T> GetModelAsync<T>(Guid id) where T : IReadModel, new()
        {
            var model = await GetModelNoDefaultAsync<T>(id);
            return model != null ? model : new T { Id = id };
        }

        public async Task<T> GetModelNoDefaultAsync<T>(Guid id) where T : IReadModel, new()
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            try
            {
                return (await _db.GetCollection<T>(typeof(T).Name).FindAsync(filter)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error on getting data from MongoDB. Exception={ex} Id={id}");
                throw;
            }
        }

        public async Task SaveModelAsync<T>(T model) where T : IReadModel, new()
        {
            model.LastModified = DateTime.UtcNow;
            model.Version += 1;

            var filter = Builders<T>.Filter.And(
                Builders<T>.Filter.Eq(x => x.Id, model.Id),
                Builders<T>.Filter.Lt(x => x.Version, model.Version)
            );

            var options = new ReplaceOptions
            {
                IsUpsert = true
            };

            try
            {
                await _db.GetCollection<T>(typeof(T).Name).ReplaceOneAsync(filter, model, options);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error on saving data to MongoDB. Exception={ex} Model={model}");
                throw;
            }
        }

    }
}
