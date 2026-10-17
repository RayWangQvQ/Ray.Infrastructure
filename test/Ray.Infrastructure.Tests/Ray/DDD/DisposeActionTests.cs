using Ray.DDD;
using Shouldly;
using Xunit;

namespace Ray.Infrastructure.Tests.Ray.DDD
{
    public class DisposeActionTests
    {
        [Fact]
        public void Constructor_WithAction_ShouldCreateInstance()
        {
            // Arrange
            var actionExecuted = false;
            Action testAction = () => actionExecuted = true;

            // Act
            var disposeAction = new DisposeAction(testAction);

            // Assert
            disposeAction.ShouldNotBeNull();
            actionExecuted.ShouldBeFalse(); // Action should not be executed yet
        }

        [Fact]
        public void Dispose_ShouldExecuteProvidedAction()
        {
            // Arrange
            var actionExecuted = false;
            Action testAction = () => actionExecuted = true;
            var disposeAction = new DisposeAction(testAction);

            // Act
            disposeAction.Dispose();

            // Assert
            actionExecuted.ShouldBeTrue();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldExecuteActionMultipleTimes()
        {
            // Arrange
            var executionCount = 0;
            Action testAction = () => executionCount++;
            var disposeAction = new DisposeAction(testAction);

            // Act
            disposeAction.Dispose();
            disposeAction.Dispose();
            disposeAction.Dispose();

            // Assert
            executionCount.ShouldBe(3);
        }

        [Fact]
        public void UsingStatement_ShouldAutomaticallyCallDispose()
        {
            // Arrange
            var actionExecuted = false;
            Action testAction = () => actionExecuted = true;

            // Act
            using (var disposeAction = new DisposeAction(testAction))
            {
                actionExecuted.ShouldBeFalse(); // Not executed yet
            } // Dispose should be called here

            // Assert
            actionExecuted.ShouldBeTrue();
        }

        [Fact]
        public void Constructor_WithNullAction_ShouldNotThrow()
        {
            // Act & Assert
            var act = () => new DisposeAction(null);
            act.ShouldNotThrow();
        }

        [Fact]
        public void Dispose_WithNullAction_ShouldThrowNullReferenceException()
        {
            // Arrange
            var disposeAction = new DisposeAction(null);

            // Act & Assert
            var act = () => disposeAction.Dispose();
            act.ShouldThrow<NullReferenceException>();
        }
    }
}
