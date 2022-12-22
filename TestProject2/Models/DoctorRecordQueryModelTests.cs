using FluentAssertions;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Api.Tests.Models
{
    public class DoctorRecordQueryModelTests
    {
        public static readonly object[] ids =
{
            new ObjectId("675849586758495867584958").ToString(),
            new ObjectId("589308659386596854368666").ToString()
        };

        [Theory]
        [InlineData(nameof(ids))]
        public void From_Name_MappedCorrectly(string id)
        {
            var data = new DoctorReadModel
            {
                _id = id,
            };

            var response = DoctorRecordQueryResponse.From(data);

            response.DoctorId.Should().Be(id);
        }

    }
}
