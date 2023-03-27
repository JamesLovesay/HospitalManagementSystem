using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Queries.Patients
{
    public class PatientsQueryHandler : IRequestHandler<PatientsQuery, PatientsQueryResponse>
    {
        private readonly IPatientsRepository _repository;

        public PatientsQueryHandler(IPatientsRepository repository)
        {
            _repository = repository;
        }

        public async Task<PatientsQueryResponse> Handle(PatientsQuery request, CancellationToken cancellationToken)
        {
            PatientsQueryModel query = new PatientsQueryModel(request);

            var (patients, detail) = await _repository.GetPatients(query);

            return new PatientsQueryResponse
            {
                Patients = patients.Select(x => Patient.From(x)).ToList(),
                Detail = detail
            };
        }
    }
}
