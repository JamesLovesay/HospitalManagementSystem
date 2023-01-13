namespace HospitalManagementSystem.Infra.MongoDBStructure.Interfaces
{
    public interface IReadModel
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        DateTime LastModified { get; set; }
        int Version { get; set; }
    }
}
