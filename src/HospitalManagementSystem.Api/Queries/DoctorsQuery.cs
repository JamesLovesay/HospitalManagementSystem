using HospitalManagementSystem.Api.Validators;
using MediatR;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api.Queries
{
    public class DoctorsQuery : IRequest<DoctorsQueryResponse>
    {
        public List<ObjectId>? DoctorId { get; set; }
        //[Range(0, int.MaxValue)]
        public int? Page { get; set; }
        //[Range(0, 100)]
        public int? PageSize { get; set; }
        //[DoctorSortByValidator]
        public string? SortBy { get; set; }
        //[DoctorSortDirectionValidator]
        public string? SortDirection { get; set; }
        //[StringLength(40)]
        public string? DoctorName { get; set; }
        //[StringLength(20, MinimumLength = 1)]
        public List<string>? Status { get; set; }
        //[StringLength(20, MinimumLength = 1)]
        public List<string>? Specialism { get; set; }

    }
}
