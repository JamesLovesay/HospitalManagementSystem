using HospitalManagementSystem.Api.Helpers;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using MongoDB.Driver;
using HospitalManagementSystem.Infra.MongoDBStructure.Extensions;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using HospitalManagementSystem.Infra.MongoDBStructure;
using HospitalManagementSystem.Infra.MongoDBStructure.ReadModels;
using HospitalManagementSystem.Api.Models.Doctors;
using HospitalManagementSystem.Api.Queries.Doctors;

namespace HospitalManagementSystem.Api.Repositories
{
    public class DoctorsRepository : ReadStore, IDoctorsRepository
    {
        public DoctorsRepository(
            IMongoFactory mongoFactory,
            Serilog.ILogger logger) : base(mongoFactory, logger)
        {
        }

        public async Task<(List<DoctorReadModel> doctors, DoctorsQueryDetail detail)> GetDoctors(DoctorsQueryModel query)
        {
            var page = (query.Page ?? 1) < 1 ? 1 : query.Page ?? 1;
            var pageSize = (query.PageSize ?? QueryHelper.DefaultPageSize) < 1 ? QueryHelper.DefaultPageSize : query.PageSize ?? QueryHelper.DefaultPageSize;

            var filters = new List<FilterDefinition<DoctorReadModel>>();

            filters.AddFilter((x) => x._id, query.DoctorId);
            filters.AddFilter((x) => x.Specialism, query.Specialisms);
            filters.AddFilter((x) => x.Status, query.Status);
            filters.AddFilter((x) => x.Name, query.Name);

            var filter = filters.Consolidate();

            var (sort, sortBy, sortDirection) = QueryHelper.GetDoctorSortDetails(query.SortBy, query.SortDirection);

            var options = new FindOptions<DoctorReadModel>
            {
                Skip = (page - 1) * pageSize,
                Limit = pageSize,
                Sort = sort
            };

            var countResult = await _db.GetCollection<DoctorReadModel>(typeof(DoctorReadModel).Name).CountDocumentsAsync(filter);
            var result = await _db.GetCollection<DoctorReadModel>(typeof(DoctorReadModel).Name).FindAsync(filter, options);

            var resultDetail = new DoctorsQueryDetail
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = (int)countResult,
                TotalPages = (int)countResult / (int)pageSize + (countResult % pageSize > 0 ? 1 : 0),
                SortBy = sortBy,
                SortDirection = sortDirection,
                Specialisms = query.Specialisms?.Select(x => $"{x}")?.ToList(),
                DoctorId = query.DoctorId,
                Name = query.Name,
                Status = query.Status?.Select(x => $"{x}").ToList(),
            };

            return (result.ToList(), resultDetail);
        }

        public async Task UpsertDoctor(DoctorReadModel cmd)
        {
            var filter = Builders<DoctorReadModel>.Filter.Eq(x => x._id, cmd._id);
            var update = Builders<DoctorReadModel>.Update
                .Set(x => x._id, cmd._id)
                .Set(x => x.Name, cmd.Name)
                .Set(x => x.Status, cmd.Status)
                .Set(x => x.HourlyChargingRate, cmd.HourlyChargingRate)
                .Set(x => x.Specialism, cmd.Specialism);
            var options = new UpdateOptions { IsUpsert = true };

            await _db.GetCollection<DoctorReadModel>(typeof(DoctorReadModel).Name).UpdateOneAsync(filter, update, options);
        }

        public async Task<DoctorReadModel> GetDoctorById(string doctorId)
        {
            var filter = Builders<DoctorReadModel>.Filter.Eq(x => x._id, doctorId);

            return (await _db.GetCollection<DoctorReadModel>(typeof(DoctorReadModel).Name).FindAsync(filter)).FirstOrDefault();
        }

        public async Task DeleteDoctor(string doctorId)
        {
            var doctorIdFilter = Builders<DoctorReadModel>.Filter.Eq(x => x._id, doctorId);

            await _db.GetCollection<DoctorReadModel>(typeof(DoctorReadModel).Name).DeleteOneAsync(doctorIdFilter);
        }

        // unused method

        //public async Task PublishAsync<T>(Guid modelId, Action<T> action) where T : BaseReadModel, new()
        //{
        //    var model = await GetModelAsync<T>(modelId);

        //    action.Invoke(model);

        //    await SaveModelAsync(model);
        //}

    }
}
