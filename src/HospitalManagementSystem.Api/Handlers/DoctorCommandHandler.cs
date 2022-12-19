using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;
using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Handlers
{
    public class DoctorCommandHandler : IRequestHandler<CreateDoctorCommand, ObjectId?>
    {
        private readonly IDoctorsRepository _repository;

        public DoctorCommandHandler(IDoctorsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ObjectId?> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var doctorsResult = await _repository.GetDoctors(new Models.DoctorsQueryModel { });
                var filtered = doctorsResult.doctors.Where(x => x.Name == request.Name).ToList();

                if (filtered.Count != 0)
                {
                    return null;
                };

                var doctorId = ObjectId.GenerateNewId();
                await _repository.UpsertDoctor(new DoctorReadModel
                {
                    DoctorId = doctorId,
                    Name = request.Name,
                    HourlyChargingRate = request.HourlyChargingRate,
                    Status = (DoctorStatus)Enum.Parse(typeof(DoctorStatus), request.Status, true),
                    Specialism = (DoctorSpecialism)Enum.Parse(typeof(DoctorSpecialism), request.Specialism, true),
                }); ;
                return doctorId;
            }
            catch (Exception e)
            {
                throw;
            }
            throw new NotImplementedException();
        }
    }
}
