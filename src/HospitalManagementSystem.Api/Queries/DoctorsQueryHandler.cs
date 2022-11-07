using HospitalManagementSystem.Api.Models;
using MediatR;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQueryHandler : IRequestHandler<DoctorsQuery, DoctorsQueryResponse>
    {
        private readonly IBroadcastingRepository _repository;

        public DoctorsQueryHandler(IBroadcastingRepository repository)
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
