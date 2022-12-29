using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Handlers
{
    public class UpdateDoctorCommandHandler : IRequestHandler<DoctorUpdateCommand, bool>
    {
        private readonly IDoctorsRepository _repository;

        public UpdateDoctorCommandHandler(IDoctorsRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DoctorUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var doctor = await _repository.GetDoctorById(request.DoctorId);
                if (doctor != null)
                {
                    if (!string.IsNullOrWhiteSpace(request.Name)) doctor.Name = request.Name;
                    if (!string.IsNullOrWhiteSpace(request.Status)) doctor.Status = request.Status;
                    if (request.HourlyChargingRate != null) doctor.HourlyChargingRate = (decimal)request.HourlyChargingRate;

                    await _repository.UpsertDoctor(doctor);
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
