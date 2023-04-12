using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Models.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;
using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Handlers.Patients
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, ObjectId?>
    {
        private readonly IPatientsRepository _repository;

        public CreatePatientCommandHandler(IPatientsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ObjectId?> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patientsResult = await _repository.GetPatients(new PatientsQueryModel { });
                var filtered = new List<PatientReadModel>();
                if (patientsResult.patients != null)
                {
                    filtered = patientsResult.patients.Where(x => x.Name == $"{request.FirstName} + {request.LastName}")
                                                      .Where(x => x.DateOfBirth == request.DateOfBirth).ToList() ?? new List<PatientReadModel>();
                }

                if (filtered.Count != 0)
                {
                    throw new PatientExistsException("Patient with this name already exists.");
                };

                var patientId = ObjectId.GenerateNewId();
                await _repository.UpsertPatient(new PatientReadModel
                {
                    _id = patientId.ToString(),
                    Name = request.FirstName + request.LastName,
                    Gender = request.Gender,
                    DateOfBirth = request.DateOfBirth,
                    AdmissionDate = request.AdmissionDate,
                    IsAdmitted = request.IsAdmitted,
                    PatientStatus = request.Status,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    RoomId = request.RoomId,
                });
                return patientId;
            }
            catch (Exception e)
            {
                throw new Exception($"Error when creating patient {request.FirstName} {request.LastName}");
            }
        }
    }
}
