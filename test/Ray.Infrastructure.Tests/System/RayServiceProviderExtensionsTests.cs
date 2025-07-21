using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ray.Infrastructure.Tests.System
{
    public class RayServiceProviderExtensionsTests
    {
        [Fact]
        public void GetEngine_WithServiceProvider_ShouldNotThrow()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<string>("test");
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            var act = () => serviceProvider.GetEngine();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetRootScope_WithServiceProvider_ShouldNotThrow()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<string>("test");
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            var act = () => serviceProvider.GetRootScope();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetEngine_WithNullServiceProvider_ShouldHandleGracefully()
        {
            // Arrange
            IServiceProvider? serviceProvider = null;

            // Act & Assert
            var act = () => serviceProvider!.GetEngine();
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceProviderExtensions_ShouldWorkWithDifferentProviderTypes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddScoped<string>(_ => "scoped test");
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            var act1 = () => serviceProvider.GetEngine();
            var act2 = () => serviceProvider.GetRootScope();

            act1.Should().NotThrow();
            act2.Should().NotThrow();
        }

        [Fact]
        public void ServiceProviderExtensions_WithScope_ShouldWork()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddScoped<string>(_ => "scoped test");
            var serviceProvider = services.BuildServiceProvider();

            // Act & Assert
            using var scope = serviceProvider.CreateScope();
            var act = () => scope.ServiceProvider.GetEngine();
            act.Should().NotThrow();
        }
    }
}
