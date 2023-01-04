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
        public void From_Id_MappedCorrectly(string id)
        {
            var data = new DoctorReadModel
            {
                _id = id,
            };

            var response = DoctorRecordQueryResponse.From(data);

            response.DoctorId.Should().Be(id);
        }

        [Theory]
        [InlineData("test A")]
        [InlineData("test B")]
        public void From_Name_MappedCorrectly(string name)
        {
            var data = new DoctorReadModel
            {
                Name = name,
            };

            var response = DoctorRecordQueryResponse.From(data);

            response.DoctorName.Should().Be(name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(320)]
        public void From_Rate_MappedCorrectly(decimal rate)
        {
            var data = new DoctorReadModel
            {
                HourlyChargingRate = rate,
            };

            var response = DoctorRecordQueryResponse.From(data);

            response.HourlyChargingRate.Should().Be(rate);
        }

        [Theory]
        [InlineData("inactive")]
        [InlineData("activepermanent")]
        public void From_Status_MappedCorrectly(string status)
        {
            var data = new DoctorReadModel
            {
                Status = status,
            };

            var response = DoctorRecordQueryResponse.From(data);

            response.Status.Should().Be(status);
        }

        [Theory]
        [InlineData("orthopaedics")]
        [InlineData("neurology")]
        public void From_Specialism_MappedCorrectly(string specialism)
        {
            var data = new DoctorReadModel
            {
                Specialism = specialism,
            };

            var response = DoctorRecordQueryResponse.From(data);

            response.Specialism.Should().Be(specialism);
        }

    }
}
