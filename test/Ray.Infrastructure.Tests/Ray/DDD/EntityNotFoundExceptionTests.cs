using Ray.DDD;
using Shouldly;
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
            exception.Message.ShouldBe(errorMessage);
            exception.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public void Constructor_WithNullMessage_ShouldNotThrow()
        {
            // Act & Assert
            var act = () => new EntityNotFoundException(null);
            act.ShouldNotThrow();
        }

        [Fact]
        public void Constructor_WithEmptyMessage_ShouldSetEmptyMessage()
        {
            // Arrange
            var emptyMessage = "";

            // Act
            var exception = new EntityNotFoundException(emptyMessage);

            // Assert
            exception.Message.ShouldBe(emptyMessage);
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
            exception.Message.ShouldBe(message);
        }

        [Fact]
        public void Exception_ShouldInheritFromException()
        {
            // Arrange
            var exception = new EntityNotFoundException("Test message");

            // Assert
            exception.ShouldBeAssignableTo<Exception>();
        }
    }
}
