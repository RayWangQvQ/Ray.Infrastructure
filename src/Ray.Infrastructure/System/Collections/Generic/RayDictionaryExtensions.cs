using System.Text;

namespace System.Collections.Generic;

public static class RayDictionaryExtensions
{
    public static void AddIfNotExist<TKey, TValue>(this Dictionary<TKey, TValue> dic, Dictionary<TKey, TValue> addDic)
    {
        foreach (var item in addDic)
        {
            dic.AddIfNotExist(item.Key, item.Value);
        }
    }

    public static void AddIfNotExist<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
    {
        if (dic.ContainsKey(key)) return;
        dic.Add(key, value);
    }

    public static string GetSelectionToStr(this Dictionary<string, string> dic)
    {
        var sb = new StringBuilder();
        foreach (var item in dic)
        {
            sb.AppendLine($"{item.Key}：{item.Value}");
        }

        return sb.ToString();
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
    {
        TValue obj;
        if (dictionary.TryGetValue(key, out obj))
        {
            return obj;
        }

        return dictionary[key] = factory(key);
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
    {
        return dictionary.GetOrAdd(key, k => factory());
    }
}
