using System.Text.Json;

namespace System;

public static class RayStringExtensions
{
    #region Json

    public static T JsonDeserialize<T>(this string str)
    {
        return JsonSerializer.Deserialize<T>(str);
    }

    /// <summary>json格式化</summary>
    /// <param name="str">The string.</param>
    /// <returns>System.String.</returns>
    public static string AsFormatJsonStr(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        using var doc = JsonDocument.Parse(str);
        return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
    }

    #endregion
}
