using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Ray.Infrastructure.Tests.System
{
    public class RayObjectExtensionsTests
    {
        private class TestClass
        {
            public string PublicProperty { get; set; } = "PublicValue";
            private string PrivateField = "PrivateValue";
            internal string InternalProperty { get; set; } = "InternalValue";
            protected string ProtectedField = "ProtectedValue";
            public static string StaticProperty { get; set; } = "StaticValue";
        }

        [Fact]
        public void FlagsOfAll_ShouldIncludeAllBindingFlags()
        {
            // Act
            var flags = RayObjectExtensions.FlagsOfAll;

            // Assert
            flags.Should().HaveFlag(BindingFlags.NonPublic);
            flags.Should().HaveFlag(BindingFlags.Public);
            flags.Should().HaveFlag(BindingFlags.Static);
            flags.Should().HaveFlag(BindingFlags.Instance);
        }

        [Fact]
        public void FlagsOfAllCurrent_ShouldIncludeAllAndDeclaredOnly()
        {
            // Act
            var flags = RayObjectExtensions.FlagsOfAllCurrent;

            // Assert
            flags.Should().HaveFlag(BindingFlags.NonPublic);
            flags.Should().HaveFlag(BindingFlags.Public);
            flags.Should().HaveFlag(BindingFlags.Static);
            flags.Should().HaveFlag(BindingFlags.Instance);
            flags.Should().HaveFlag(BindingFlags.DeclaredOnly);
        }

        [Fact]
        public void FlagsOfAllPulic_ShouldIncludePublicAndInstance()
        {
            // Act
            var flags = RayObjectExtensions.FlagsOfAllPulic;

            // Assert
            flags.Should().HaveFlag(BindingFlags.Public);
            flags.Should().HaveFlag(BindingFlags.Instance);
            flags.Should().NotHaveFlag(BindingFlags.NonPublic);
            flags.Should().NotHaveFlag(BindingFlags.Static);
        }

        [Fact]
        public void FlagsOfAllPulicCurrent_ShouldIncludePublicInstanceAndDeclaredOnly()
        {
            // Act
            var flags = RayObjectExtensions.FlagsOfAllPulicCurrent;

            // Assert
            flags.Should().HaveFlag(BindingFlags.Public);
            flags.Should().HaveFlag(BindingFlags.Instance);
            flags.Should().HaveFlag(BindingFlags.DeclaredOnly);
            flags.Should().NotHaveFlag(BindingFlags.NonPublic);
            flags.Should().NotHaveFlag(BindingFlags.Static);
        }

        [Fact]
        public void BindingFlags_ShouldProvideCorrectCombinations()
        {
            // Arrange
            var testObject = new TestClass();
            var type = testObject.GetType();

            // Act & Assert - Test different flag combinations
            var allMembers = type.GetMembers(RayObjectExtensions.FlagsOfAll);
            var publicMembers = type.GetMembers(RayObjectExtensions.FlagsOfAllPulic);
            var currentMembers = type.GetMembers(RayObjectExtensions.FlagsOfAllCurrent);

            allMembers.Length.Should().BeGreaterThan(publicMembers.Length);
            currentMembers.Should().NotBeEmpty();
        }

        [Fact]
        public void BindingFlags_Properties_ShouldHaveCorrectValues()
        {
            // Assert
            RayObjectExtensions
                .FlagsOfAll.Should()
                .Be(
                    BindingFlags.NonPublic
                        | BindingFlags.Public
                        | BindingFlags.Static
                        | BindingFlags.Instance
                );

            RayObjectExtensions
                .FlagsOfAllCurrent.Should()
                .Be(
                    BindingFlags.NonPublic
                        | BindingFlags.Public
                        | BindingFlags.Static
                        | BindingFlags.Instance
                        | BindingFlags.DeclaredOnly
                );

            RayObjectExtensions
                .FlagsOfAllPulic.Should()
                .Be(BindingFlags.Public | BindingFlags.Instance);

            RayObjectExtensions
                .FlagsOfAllPulicCurrent.Should()
                .Be(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
    }
}
