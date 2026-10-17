using Ray.DDD;
using Shouldly;
using Xunit;

namespace Ray.Infrastructure.Tests.Ray.DDD
{
    public class DataFilterStateTests
    {
        [Fact]
        public void Constructor_WithTrueValue_ShouldSetIsEnabledToTrue()
        {
            // Arrange & Act
            var state = new DataFilterState(true);

            // Assert
            state.IsEnabled.ShouldBeTrue();
        }

        [Fact]
        public void Constructor_WithFalseValue_ShouldSetIsEnabledToFalse()
        {
            // Arrange & Act
            var state = new DataFilterState(false);

            // Assert
            state.IsEnabled.ShouldBeFalse();
        }

        [Fact]
        public void IsEnabled_ShouldBeSettable()
        {
            // Arrange
            var state = new DataFilterState(false);

            // Act
            state.IsEnabled = true;

            // Assert
            state.IsEnabled.ShouldBeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Constructor_WithVariousValues_ShouldSetCorrectly(bool expectedValue)
        {
            // Arrange & Act
            var state = new DataFilterState(expectedValue);

            // Assert
            state.IsEnabled.ShouldBe(expectedValue);
        }
    }
}
