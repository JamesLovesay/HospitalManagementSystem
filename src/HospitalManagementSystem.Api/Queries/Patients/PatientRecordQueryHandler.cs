using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Queries.Patients
{
    public class PatientRecordQueryHandler : IRequestHandler<PatientRecordQuery, PatientRecordQueryResponse>
    {
        private readonly IPatientsRepository _repository;

        public PatientRecordQueryHandler(IPatientsRepository repository)
        {
            _repository = repository;
        }

        public async Task<PatientRecordQueryResponse> Handle(PatientRecordQuery request, CancellationToken cancellationToken)
        {
            var patient = await _repository.GetPatientById(request.PatientId);

            if (patient == null || patient._id == null)
            {
                return PatientRecordQueryResponse.Empty();
            }
            else
            {
                return PatientRecordQueryResponse.From(patient);
            }
        }
    }
}
