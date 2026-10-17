using System.Text.Json;
using Shouldly;
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
            var json = JsonSerializer.Serialize(testObject);

            // Act
            var result = json.JsonDeserialize<object>();

            // Assert
            result.ShouldNotBeNull();
            // 验证反序列化成功即可，不测试具体属性值
        }

        [Fact]
        public void JsonDeserialize_WithInvalidJson_ShouldThrowException()
        {
            // Arrange
            var invalidJson = "{ invalid json }";

            // Act & Assert
            var act = () => invalidJson.JsonDeserialize<object>();
            act.ShouldThrow<JsonException>();
        }

        [Fact]
        public void AsFormatJsonStr_WithValidJson_ShouldReturnFormattedJson()
        {
            // Arrange
            var compactJson = "{\"name\":\"test\",\"age\":25}";

            // Act
            var result = compactJson.AsFormatJsonStr();

            // Assert
            result.ShouldNotBeNull();
            result.ShouldContain("  \"name\":"); // 检查是否包含缩进
            result.ShouldContain("\"name\": \"test\"");
            result.ShouldContain("\"age\": 25");
        }

        [Fact]
        public void AsFormatJsonStr_WithInvalidJson_ShouldThrowException()
        {
            // Arrange
            var invalidJson = "invalid json";

            // Act & Assert
            // AsFormatJsonStr在遇到无效JSON时会抛出异常，这是预期行为
            var act = () => invalidJson.AsFormatJsonStr();
            act.ShouldThrow<JsonException>();
        }

        [Fact]
        public void AsFormatJsonStr_WithEmptyString_ShouldReturnOriginalString()
        {
            // Arrange
            var emptyString = "";

            // Act
            var result = emptyString.AsFormatJsonStr();

            // Assert
            result.ShouldBe(emptyString);
        }
    }
}
