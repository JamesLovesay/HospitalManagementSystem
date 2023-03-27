using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Handlers.Patients
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, bool>
    {
        private readonly IPatientsRepository _repository;

        public DeletePatientCommandHandler(IPatientsRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeletePatientCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var patientReadModel = await _repository.GetPatientById(cmd.PatientId);
                if (patientReadModel != null)
                {
                    await _repository.DeletePatient(cmd.PatientId);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception($"Error whilst deleting patient {cmd.PatientId}", e.InnerException);
            }
        }
    }
}
