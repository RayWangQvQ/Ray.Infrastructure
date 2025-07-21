using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Ray.Infrastructure.AutoTask;

public class AutoTaskTypeFactory
{
    private Dictionary<int, AutoTaskInfo> _dic = new Dictionary<int, AutoTaskInfo>();

    public AutoTaskTypeFactory(List<Type> types)
    {
        var index = 1;
        foreach (var type in types)
        {
            var autoTaskAttr = type.GetCustomAttribute<AutoTaskAttribute>();
            if (autoTaskAttr == null)
                continue;

            var info = new AutoTaskInfo(autoTaskAttr.Code, autoTaskAttr.Alias, type);
            _dic[index++] = info;
        }
    }

    public void Show(ILogger logger)
    {
        foreach (var item in _dic)
        {
            logger.LogInformation("{index}):{code}", item.Key, item.Value.ToString());
        }
    }

    public AutoTaskInfo GetByIndex(int index)
    {
        return _dic[index];
    }

    public AutoTaskInfo GetByCode(string code)
    {
        return _dic.Values.FirstOrDefault(x =>
            string.Equals(x.Code, code, StringComparison.CurrentCultureIgnoreCase)
        );
    }
}

public class AutoTaskInfo
{
    public AutoTaskInfo(string code, string alias, Type implementType)
    {
        Code = code;
        Alias = alias;
        ImplementType = implementType;
    }

    public string Code { get; set; }

    public string Alias { get; set; }

    public Type ImplementType { get; set; }

    public override string ToString()
    {
        return $"{Code}({Alias})";
    }
}
