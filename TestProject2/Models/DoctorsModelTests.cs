using FluentAssertions;
using HospitalManagementSystem.Api.Models;
using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Tests.Models
{
    public class DoctorsModelTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Dr A")]
        public void From_Name_MappedCorrectly(string name)
        {
            var data = new DoctorReadModel
            {
                Name = name
            };

            var response = Doctor.From(data);

            response.Name.Should().Be(name);
        }

        [Theory]
        [InlineData(DoctorSpecialism.GeneralPractice)]
        [InlineData(DoctorSpecialism.NotKnown)]
        [InlineData(DoctorSpecialism.GeneralSurgery)]
        [InlineData(DoctorSpecialism.Orthopaedics)]
        [InlineData(DoctorSpecialism.Psychiatry)]
        [InlineData(DoctorSpecialism.Gynaecology)]
        [InlineData(DoctorSpecialism.Urology)]
        [InlineData(DoctorSpecialism.Psychology)]
        [InlineData(DoctorSpecialism.Neurology)]
        public void From_Specialism_MappedCorrectly(DoctorSpecialism specialism)
        {
            var data = new DoctorReadModel
            {
                Specialism = specialism.ToString()
            };

            var response = Doctor.From(data);

            response.Specialism.Should().Be(specialism);
        }

        [Theory]
        [InlineData(DoctorStatus.ActivePermanent)]
        [InlineData(DoctorStatus.ActiveVisiting)]
        [InlineData(DoctorStatus.Inactive)]

        public void From_Status_MappedCorrectly(DoctorStatus status)
        {
            var data = new DoctorReadModel
            {
                Status = status.ToString()
            };

            var response = Doctor.From(data);

            response.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(900)]

        public void From_HourlyChargingRate_MappedCorrectly(int rate)
        {
            var data = new DoctorReadModel
            {
                HourlyChargingRate = rate
            };

            var response = Doctor.From(data);

            response.HourlyChargingRate.Should().Be(rate);
        }

        public static readonly object[][] ids =
{
            new object[] { new ObjectId("675849586758495867584958")},
            new object[] { new ObjectId("589308659386596854368666")},
        };

        [Theory]
        [MemberData(nameof(ids))]

        public void From_ID_MappedCorrectly(ObjectId id)
        {
            var data = new DoctorReadModel
            {
                _id = id.ToString(),
            };

            var response = Doctor.From(data);

            response.DoctorId.Should().Be(id.ToString());
        }
    }
}
