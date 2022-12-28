using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Handlers
{
    public class DeleteDoctorCommandHandler : IRequestHandler<DoctorDeleteCommand, bool>
    {
        private readonly IDoctorsRepository _repository;

        public DeleteDoctorCommandHandler(IDoctorsRepository repository)
        {
            _repository = repository;
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
