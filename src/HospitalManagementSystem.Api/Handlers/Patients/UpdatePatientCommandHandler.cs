using HospitalManagementSystem.Api.Commands.Patients;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MediatR;

namespace HospitalManagementSystem.Api.Handlers.Patients
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, bool>
    {
        private readonly IPatientsRepository _repository;

        public UpdatePatientCommandHandler(IPatientsRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _repository.GetPatientById(request.PatientId);
                if (patient != null)
                {
                    if (!string.IsNullOrWhiteSpace(request.Name)) patient.Name = request.Name;
                    if (!string.IsNullOrWhiteSpace(request.DateOfBirth)) patient.DateOfBirth = request.DateOfBirth;
                    if (!string.IsNullOrWhiteSpace(request.AdmissionDate)) patient.AdmissionDate = request.AdmissionDate;
                    if (!string.IsNullOrWhiteSpace(request.Gender)) patient.Gender = request.Gender;
                    if (!string.IsNullOrWhiteSpace(request.Status)) patient.PatientStatus = request.Status;
                    if (!string.IsNullOrWhiteSpace(request.PhoneNumber)) patient.PhoneNumber = request.PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(request.Email)) patient.Email = request.Email;
                    if(request.IsAdmitted != null) patient.IsAdmitted = (bool)request.IsAdmitted;
                    if(request.RoomId != null) patient.RoomId = (int)request.RoomId;

                    await _repository.UpsertPatient(patient);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception($"Error when updating patient with Id {request.PatientId}");
            }
        }
    }
}
