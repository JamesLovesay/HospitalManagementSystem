namespace HospitalManagementSystem.Api.Models
{
    public class HospitalManagementSystemDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string DoctorsCollectionName { get; set; } = null!;
    }
}
