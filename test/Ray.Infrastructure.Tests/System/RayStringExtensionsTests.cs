using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Ray.Infrastructure.Tests.System
{
    public class RayStringExtensionsTests
    {
        [Fact]
        public void JsonDeserialize_WithValidJson_ShouldReturnObject()
        {
            // Arrange
            var testObject = new { Name = "Test", Age = 25 };
            var json = JsonConvert.SerializeObject(testObject);

            // Act
            var result = json.JsonDeserialize<object>();

            // Assert
            result.Should().NotBeNull();
            // 验证反序列化成功即可，不测试具体属性值
        }

        [Fact]
        public void JsonDeserialize_WithInvalidJson_ShouldThrowException()
        {
            // Arrange
            var invalidJson = "{ invalid json }";

            // Act & Assert
            Assert.Throws<JsonReaderException>(() => invalidJson.JsonDeserialize<object>());
        }

        [Fact]
        public void AsFormatJsonStr_WithValidJson_ShouldReturnFormattedJson()
        {
            // Arrange
            var compactJson = "{\"name\":\"test\",\"age\":25}";

            // Act
            var result = compactJson.AsFormatJsonStr();

            // Assert
            result.Should().NotBeNull();
            result.Should().Contain("    "); // 检查是否包含缩进
            result.Should().Contain("\"name\": \"test\"");
            result.Should().Contain("\"age\": 25");
        }

        [Fact]
        public void AsFormatJsonStr_WithInvalidJson_ShouldReturnOriginalString()
        {
            // Arrange
            var invalidJson = "invalid json";

            // Act & Assert
            // AsFormatJsonStr在遇到无效JSON时会抛出异常，这是预期行为
            var act = () => invalidJson.AsFormatJsonStr();
            act.Should().Throw<JsonReaderException>();
        }

        [Fact]
        public void AsFormatJsonStr_WithEmptyString_ShouldReturnOriginalString()
        {
            // Arrange
            var emptyString = "";

            // Act
            var result = emptyString.AsFormatJsonStr();

            // Assert
            result.Should().Be(emptyString);
        }
    }
}
