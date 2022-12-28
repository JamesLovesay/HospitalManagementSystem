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
                var filtered = new List<DoctorReadModel>();
                if (doctorsResult.doctors != null)
                {
                    filtered = doctorsResult.doctors.Where(x => x.Name == request.Name).ToList() ?? new List<DoctorReadModel>();
                }

                if (filtered.Count != 0)
                {
                    return null;
                };

                var doctorId = ObjectId.GenerateNewId();
                await _repository.UpsertDoctor(new DoctorReadModel
                {
                    _id = doctorId.ToString(),
                    Name = request.Name,
                    HourlyChargingRate = request.HourlyChargingRate,
                    Status = request.Status,
                    Specialism = request.Specialism,
                }); ;
                return doctorId;
            }
            catch (Exception e)
            {
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> Handle(DoctorDeleteCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var doctorReadModel = await _repository.GetDoctorById(cmd.DoctorId);
                if (doctorReadModel != null)
                {
                    await _repository.DeleteDoctor(cmd.DoctorId);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
