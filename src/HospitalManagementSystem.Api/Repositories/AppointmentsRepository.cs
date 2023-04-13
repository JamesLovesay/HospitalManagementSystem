using HospitalManagementSystem.Api.Models.Appointments;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Infra.MongoDBStructure;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using MongoDB.Driver;

namespace HospitalManagementSystem.Api.Repositories;

public class AppointmentsRepository : ReadStore, IAppointmentRepository
{
    public AppointmentsRepository(
            IMongoFactory mongoFactory,
            Serilog.ILogger logger) : base(mongoFactory, logger)
    {
    }

    public async Task<AppointmentReadModel?> GetAppointmentById(int id)
    {
        var filter = Builders<AppointmentReadModel>.Filter.Eq(a => a.Id, id);
        return (await _db.GetCollection<AppointmentReadModel>(typeof(AppointmentReadModel).Name).FindAsync(filter)).FirstOrDefault();
    }
}
