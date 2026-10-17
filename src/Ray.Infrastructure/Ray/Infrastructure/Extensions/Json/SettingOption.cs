using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Ray.Infrastructure.Extensions.Json;

public class SettingOption
{
    public JsonSerializerOptions SerializerSettings { get; set; }

    /// <summary>
    /// 忽略或只保留部分属性
    /// </summary>
    public FilterPropsOption FilterProps { get; set; }

    /// <summary>
    /// 枚举序列化为字符串
    /// </summary>
    public bool EnumToString { get; set; } = false;

    /// <summary>
    /// 构建
    /// </summary>
    /// <returns></returns>
    public JsonSerializerOptions BuildSettings()
    {
        if (SerializerSettings == null)
            SerializerSettings = new JsonSerializerOptions();

        //忽略/只保留部分属性
        if (FilterProps != null)
        {
            var props = FilterProps.Props;
            var retain = FilterProps.FilterEnum;
            SerializerSettings.TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    typeInfo =>
                    {
                        if (typeInfo.Kind != JsonTypeInfoKind.Object)
                            return;
                        foreach (
                            var prop in typeInfo
                                .Properties.Where(p =>
                                    retain == FilterEnum.Ignore
                                        ? !props.Contains(p.Name)
                                        : props.Contains(p.Name)
                                )
                                .ToList()
                        )
                        {
                            typeInfo.Properties.Remove(prop);
                        }
                    },
                },
            };
        }

        //枚举处理
        if (EnumToString)
            SerializerSettings.Converters.Add(new JsonStringEnumConverter());

        return SerializerSettings;
    }
}
