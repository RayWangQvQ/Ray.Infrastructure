using FluentAssertions;
using Ray.DDD;
using Xunit;

namespace Ray.Infrastructure.Tests.Ray.DDD
{
    public class EntityNotFoundExceptionTests
    {
        [Fact]
        public void Constructor_WithMessage_ShouldSetMessage()
        {
            // Arrange
            var errorMessage = "Entity not found with ID: 123";

            // Act
            var exception = new EntityNotFoundException(errorMessage);

            // Assert
            exception.Message.Should().Be(errorMessage);
            exception.Should().BeAssignableTo<Exception>();
        }

        [Fact]
        public void Constructor_WithNullMessage_ShouldNotThrow()
        {
            // Act & Assert
            var act = () => new EntityNotFoundException(null);
            act.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithEmptyMessage_ShouldSetEmptyMessage()
        {
            // Arrange
            var emptyMessage = "";

            // Act
            var exception = new EntityNotFoundException(emptyMessage);

            // Assert
            exception.Message.Should().Be(emptyMessage);
        }

        [Theory]
        [InlineData("User not found")]
        [InlineData("Product with ID 456 does not exist")]
        [InlineData("订单未找到")]
        public void Constructor_WithVariousMessages_ShouldSetCorrectMessage(string message)
        {
            // Act
            var exception = new EntityNotFoundException(message);

            // Assert
            exception.Message.Should().Be(message);
        }

        [Fact]
        public void Exception_ShouldInheritFromException()
        {
            // Arrange
            var exception = new EntityNotFoundException("Test message");

            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }
    }
}
