using HospitalManagementSystem.Api.Events.Appointments;
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
        if (id <= 0) throw new ArgumentException("Appointment ID cannot be negative");

        var filter = Builders<AppointmentReadModel>.Filter.Eq(a => a.Id, id);
        return (await _db.GetCollection<AppointmentReadModel>(typeof(AppointmentReadModel).Name).FindAsync(filter)).FirstOrDefault();
    }

    public async Task<Guid> AddAppointment(Appointment appointment)
    {
        if (appointment == null) throw new ArgumentNullException(nameof(appointment));

        await _db.GetCollection<Appointment>(typeof(Appointment).Name).InsertOneAsync(appointment);

        return appointment.Id;
    }
}
