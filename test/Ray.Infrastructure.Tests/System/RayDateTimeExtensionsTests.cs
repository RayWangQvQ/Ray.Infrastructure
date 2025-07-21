using FluentAssertions;
using Xunit;

namespace Ray.Infrastructure.Tests.System
{
    public class RayDateTimeExtensionsTests
    {
        [Fact]
        public void LastDayOfMonth_WithJanuaryDate_ShouldReturnJanuary31()
        {
            // Arrange
            var date = new DateTime(2023, 1, 15);

            // Act
            var result = date.LastDayOfMonth();

            // Assert
            result.Should().Be(new DateTime(2023, 1, 31));
        }

        [Fact]
        public void LastDayOfMonth_WithFebruaryInLeapYear_ShouldReturnFebruary29()
        {
            // Arrange
            var date = new DateTime(2024, 2, 10); // 2024 is a leap year

            // Act
            var result = date.LastDayOfMonth();

            // Assert
            result.Should().Be(new DateTime(2024, 2, 29));
        }

        [Fact]
        public void LastDayOfMonth_WithFebruaryInNonLeapYear_ShouldReturnFebruary28()
        {
            // Arrange
            var date = new DateTime(2023, 2, 10); // 2023 is not a leap year

            // Act
            var result = date.LastDayOfMonth();

            // Assert
            result.Should().Be(new DateTime(2023, 2, 28));
        }

        [Fact]
        public void LastDayOfMonth_WithDecemberDate_ShouldReturnDecember31()
        {
            // Arrange
            var date = new DateTime(2023, 12, 1);

            // Act
            var result = date.LastDayOfMonth();

            // Assert
            result.Should().Be(new DateTime(2023, 12, 31));
        }

        [Fact]
        public void ToTimeStamp_WithKnownDate_ShouldReturnCorrectTimeStamp()
        {
            // Arrange
            var date = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            var result = date.ToTimeStamp();

            // Assert
            result.Should().Be(1672531200); // Unix timestamp for 2023-01-01 00:00:00 UTC
        }

        [Fact]
        public void ToTimeStamp_WithUnixEpoch_ShouldReturnZero()
        {
            // Arrange
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            var result = epoch.ToTimeStamp();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void ToLongTimeStamp_WithKnownDate_ShouldReturnCorrectTimeStamp()
        {
            // Arrange
            var date = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            var result = date.ToLongTimeStamp();

            // Assert
            result.Should().Be(1672531200000); // Unix timestamp in milliseconds
        }

        [Fact]
        public void ToLongTimeStamp_WithUnixEpoch_ShouldReturnZero()
        {
            // Arrange
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            var result = epoch.ToLongTimeStamp();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void ToTimeStamp_WithLocalTime_ShouldConvertToUtc()
        {
            // Arrange
            var localDate = new DateTime(2023, 1, 1, 8, 0, 0, DateTimeKind.Local);

            // Act
            var result = localDate.ToTimeStamp();

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ToLongTimeStamp_WithLocalTime_ShouldConvertToUtc()
        {
            // Arrange
            var localDate = new DateTime(2023, 1, 1, 8, 0, 0, DateTimeKind.Local);

            // Act
            var result = localDate.ToLongTimeStamp();

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
