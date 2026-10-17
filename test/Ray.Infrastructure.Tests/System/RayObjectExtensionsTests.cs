using System.Reflection;
using Shouldly;
using Xunit;

namespace Ray.Infrastructure.Tests.System
{
    public class RayObjectExtensionsTests
    {
        private class TestClass
        {
            public string PublicProperty { get; set; } = "PublicValue";
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
            flags.HasFlag(BindingFlags.NonPublic).ShouldBeTrue();
            flags.HasFlag(BindingFlags.Public).ShouldBeTrue();
            flags.HasFlag(BindingFlags.Static).ShouldBeTrue();
            flags.HasFlag(BindingFlags.Instance).ShouldBeTrue();
        }

        [Fact]
        public void FlagsOfAllCurrent_ShouldIncludeAllAndDeclaredOnly()
        {
            // Act
            var flags = RayObjectExtensions.FlagsOfAllCurrent;

            // Assert
            flags.HasFlag(BindingFlags.NonPublic).ShouldBeTrue();
            flags.HasFlag(BindingFlags.Public).ShouldBeTrue();
            flags.HasFlag(BindingFlags.Static).ShouldBeTrue();
            flags.HasFlag(BindingFlags.Instance).ShouldBeTrue();
            flags.HasFlag(BindingFlags.DeclaredOnly).ShouldBeTrue();
        }

        [Fact]
        public void FlagsOfAllPulic_ShouldIncludePublicAndInstance()
        {
            // Act
            var flags = RayObjectExtensions.FlagsOfAllPulic;

            // Assert
            flags.HasFlag(BindingFlags.Public).ShouldBeTrue();
            flags.HasFlag(BindingFlags.Instance).ShouldBeTrue();
            flags.HasFlag(BindingFlags.NonPublic).ShouldBeFalse();
            flags.HasFlag(BindingFlags.Static).ShouldBeFalse();
        }

        [Fact]
        public void FlagsOfAllPulicCurrent_ShouldIncludePublicInstanceAndDeclaredOnly()
        {
            // Act
            var flags = RayObjectExtensions.FlagsOfAllPulicCurrent;

            // Assert
            flags.HasFlag(BindingFlags.Public).ShouldBeTrue();
            flags.HasFlag(BindingFlags.Instance).ShouldBeTrue();
            flags.HasFlag(BindingFlags.DeclaredOnly).ShouldBeTrue();
            flags.HasFlag(BindingFlags.NonPublic).ShouldBeFalse();
            flags.HasFlag(BindingFlags.Static).ShouldBeFalse();
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

            allMembers.Length.ShouldBeGreaterThan(publicMembers.Length);
            currentMembers.ShouldNotBeEmpty();
        }

        [Fact]
        public void BindingFlags_Properties_ShouldHaveCorrectValues()
        {
            // Assert
            RayObjectExtensions.FlagsOfAll.ShouldBe(
                BindingFlags.NonPublic
                    | BindingFlags.Public
                    | BindingFlags.Static
                    | BindingFlags.Instance
            );

            RayObjectExtensions.FlagsOfAllCurrent.ShouldBe(
                BindingFlags.NonPublic
                    | BindingFlags.Public
                    | BindingFlags.Static
                    | BindingFlags.Instance
                    | BindingFlags.DeclaredOnly
            );

            RayObjectExtensions.FlagsOfAllPulic.ShouldBe(
                BindingFlags.Public | BindingFlags.Instance
            );

            RayObjectExtensions.FlagsOfAllPulicCurrent.ShouldBe(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
            );
        }
    }
}
