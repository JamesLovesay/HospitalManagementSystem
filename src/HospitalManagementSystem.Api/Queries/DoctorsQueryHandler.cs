using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQueryHandler : IRequestHandler<DoctorsQuery, DoctorsQueryResponse>
    {
        private readonly IDoctorsRepository _repository;

        public DoctorsQueryHandler(IDoctorsRepository repository)
        {
            _repository = repository;
        }

        public async Task<DoctorsQueryResponse> Handle(DoctorsQuery request, CancellationToken cancellationToken)
        {
            var query = new DoctorsQueryModel(request);

            var doctors = await _repository.GetDoctors(query);

            return new DoctorsQueryResponse
            {
                Doctors = doctors.ToList()
            };
        }
    }
}
