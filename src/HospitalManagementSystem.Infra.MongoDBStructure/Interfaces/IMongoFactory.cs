using MongoDB.Driver;

namespace HospitalManagementSystem.Infra.MongoDBStructure.Interfaces
{
    public interface IMongoFactory
    {
        IMongoDatabase GetDatabase();
        IMongoClient GetClient();
    }
}