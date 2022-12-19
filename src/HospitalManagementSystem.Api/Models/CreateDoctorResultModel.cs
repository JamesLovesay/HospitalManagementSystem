using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Models
{
    internal class CreateDoctorResultModel : IActionResult
    {
        public ObjectId Id { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}