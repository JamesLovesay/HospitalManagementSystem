using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Api.Tests.Structure_Tests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void TestSerilogDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(ILogger), _ => Log.Logger);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Act
            var logger = serviceProvider.GetService<ILogger>();

            // Assert
            Assert.NotNull(logger);
        }
    }
}
