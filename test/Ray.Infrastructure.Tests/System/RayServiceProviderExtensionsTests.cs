using Microsoft.Extensions.DependencyInjection;
using Shouldly;
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
            act.ShouldNotThrow();
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
            act.ShouldNotThrow();
        }

        [Fact]
        public void GetEngine_WithNullServiceProvider_ShouldHandleGracefully()
        {
            // Arrange
            IServiceProvider? serviceProvider = null;

            // Act & Assert
            var act = () => serviceProvider!.GetEngine();
            act.ShouldThrow<NullReferenceException>();
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

            act1.ShouldNotThrow();
            act2.ShouldNotThrow();
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
            act.ShouldNotThrow();
        }
    }
}
