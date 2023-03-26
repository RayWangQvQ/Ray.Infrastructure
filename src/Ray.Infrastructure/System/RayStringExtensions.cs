using System.IO;
using Newtonsoft.Json;

namespace System
{
    public static class RayStringExtensions
    {
        #region Json

        public static T JsonDeserialize<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>json格式化</summary>
        /// <param name="str">The string.</param>
        /// <returns>System.String.</returns>
        public static string AsFormatJsonStr(this string str)
        {
            var jsonSerializer = new JsonSerializer();
            var jsonTextReader = new JsonTextReader(new StringReader(str));
            object obj = jsonSerializer.Deserialize(jsonTextReader);
            if (obj == null) return str;
            var stringWriter = new StringWriter();
            var jsonTextWriter1 = new JsonTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            JsonTextWriter jsonTextWriter2 = jsonTextWriter1;
            jsonSerializer.Serialize(jsonTextWriter2, obj);
            return stringWriter.ToString();
        }

        #endregion
    }
}
