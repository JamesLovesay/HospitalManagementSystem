using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorRecordQueryHandler : IRequestHandler<DoctorRecordQuery, DoctorRecordQueryResponse>
    {
        private readonly IDoctorsRepository _repository;

        public DoctorRecordQueryHandler(IDoctorsRepository repository) 
        { 
            _repository = repository;
        }

        public async Task<DoctorRecordQueryResponse> Handle(DoctorRecordQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _repository.GetDoctorById(request.DoctorId);

            return doctor == null ? DoctorRecordQueryResponse.Empty() : DoctorRecordQueryResponse.From(doctor);
        }
    }
}
