using FluentAssertions;
using HospitalManagementSystem.Api.Tests.Test_Helpers;
    
namespace HospitalManagementSystem.Api.Tests.Test_Helper_Tests
{
    public class TestHelperTests
    {
        [Fact]
        public void IsEquivalent_UnequalLists_ReturnsFalse()
        {
            var listA = new List<string> { "item1", "item3" };
            var listB = new List<string> { "item2", "item1" };

            var result = TestHelpers.IsEquivalent(listA, listB);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsEquivalent_EqualLists_ReturnsTrue()
        {
            var listA = new List<string> { "item1", "item2" };
            var listB = new List<string> { "item1", "item2" };

            var result = TestHelpers.IsEquivalent(listA, listB);

            result.Should().BeTrue();
        }
    }
}
