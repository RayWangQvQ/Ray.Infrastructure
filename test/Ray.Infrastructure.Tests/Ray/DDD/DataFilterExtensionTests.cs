using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Ray.DDD;
using Xunit;

namespace Ray.Infrastructure.Tests.Ray.DDD
{
    public class DataFilterExtensionTests
    {
        [Fact]
        public void AddDataFilter_ShouldRegisterDataFilterServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddDataFilter();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType.IsGenericType && 
                                                                 s.ServiceType.GetGenericTypeDefinition() == typeof(IDataFilter<>));
            
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor!.ImplementationType.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddDataFilter_ShouldReturnServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddDataFilter();

            // Assert
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddDataFilter_WithExistingServices_ShouldAddDataFilterService()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<string>("test");
            var initialCount = services.Count;

            // Act
            services.AddDataFilter();

            // Assert
            services.Count.Should().BeGreaterThan(initialCount);
        }
    }
}
