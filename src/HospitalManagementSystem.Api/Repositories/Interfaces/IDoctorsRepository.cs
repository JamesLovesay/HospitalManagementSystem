﻿using HospitalManagementSystem.Api.Models.Doctors;
using HospitalManagementSystem.Api.Queries.Doctors;
using HospitalManagementSystem.Infra.MongoDBStructure.ReadModels;

namespace HospitalManagementSystem.Api.Repositories.Interfaces
{
    public interface IDoctorsRepository
    {
        Task<(List<DoctorReadModel> doctors, DoctorsQueryDetail detail)> GetDoctors(DoctorsQueryModel query);

        //unused method
        //Task PublishAsync<T>(Guid modelId, Action<T> action) where T : BaseReadModel, new();
        Task UpsertDoctor(DoctorReadModel cmd);
        Task <DoctorReadModel> GetDoctorById(string doctorId);
        Task DeleteDoctor(string doctorId);
    }
}
