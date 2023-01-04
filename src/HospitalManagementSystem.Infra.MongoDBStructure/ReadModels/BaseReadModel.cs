using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;

namespace HospitalManagementSystem.Infra.MongoDBStructure.ReadModels
{
    public abstract class BaseReadModel : IReadModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; }
        public int Version { get; set; }
        public abstract bool IsEnabled();
    }
}
