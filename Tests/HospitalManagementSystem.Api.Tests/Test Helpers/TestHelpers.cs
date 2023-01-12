using FluentAssertions;

namespace HospitalManagementSystem.Api.Tests.Test_Helpers
{
    public class TestHelpers
    {
        public static bool IsEquivalent<T>(List<T> listA, List<T> listB)
        {
            try
            {
                listA.Should().BeEquivalentTo(listB);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
