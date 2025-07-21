using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ray.Infrastructure.QingLong;

public class QingLongHelper
{
    public static string GetAuthToken()
    {
        string qlDir = Environment.GetEnvironmentVariable("QL_DIR") ?? "/ql";

        string authFile = qlDir;
        if (File.Exists(Path.Combine(authFile, "data", "config/auth.json")))
        {
            authFile = Path.Combine(authFile, "data", "config/auth.json");
        }
        else if (File.Exists(Path.Combine(authFile, "config/auth.json")))
        {
            authFile = Path.Combine(authFile, "config/auth.json");
        }
        else
        {
            throw new Exception("获取青龙授权失败，auth.json文件不在");
        }

        var authJson = File.ReadAllText(authFile);

        var jb = JsonConvert.DeserializeObject<JObject>(authJson);
        var token = jb["token"]?.ToString();

        return token;
    }

    public static async Task AddOrUpdateEnvAsync(
        IQingLongApi qingLongApi,
        string key,
        string value,
        string remark = "",
        ILogger logger = null,
        CancellationToken cancellationToken = default
    )
    {
        var re = await qingLongApi.GetEnvsAsync(key);

        //不存在就新增
        if (re.Data.Count == 0)
        {
            var add = new AddQingLongEnv()
            {
                name = key,
                value = value,
                remarks = remark,
            };
            var addRe = await qingLongApi.AddEnvsAsync(new List<AddQingLongEnv> { add });
            logger?.LogInformation(addRe.Code == 200 ? "新增成功！" : addRe.ToJsonStr());
            return;
        }

        //存在一个就编辑
        if (re.Data.Count == 1)
        {
            QingLongEnv oldEnv = re.Data.First();
            logger?.LogInformation("Key已存在，执行更新");
            logger?.LogInformation("Key：{key}", oldEnv.name);
            var update = new UpdateQingLongEnv()
            {
                id = oldEnv.id,
                name = oldEnv.name,
                value = value,
                remarks = string.IsNullOrWhiteSpace(remark) ? oldEnv.remarks : remark,
            };

            var updateRe = await qingLongApi.UpdateEnvsAsync(update);
            logger?.LogInformation(updateRe.Code == 200 ? "更新成功！" : updateRe.ToJsonStr());

            return;
        }

        throw new Exception($"存在{re.Data.Count}个{key}");
    }

    /// <summary>
    /// 根据用户名密码保存States
    /// </summary>
    /// <param name="qingLongApi"></param>
    /// <param name="userName"></param>
    /// <param name="keyBeforeNum"></param>
    /// <param name="value"></param>
    /// <param name="keyAfterNum"></param>
    /// <param name="userNameKeyAfterNum"></param>
    /// <param name="logger"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task SaveStatesByUserNameAsync(
        IQingLongApi qingLongApi,
        string userName,
        string keyBeforeNum,
        string value,
        string keyAfterNum = "States",
        string userNameKeyAfterNum = "UserName",
        ILogger logger = null,
        CancellationToken cancellationToken = default
    )
    {
        //查env
        var re = await qingLongApi.GetEnvsAsync(keyBeforeNum);
        if (re.Code != 200)
        {
            logger?.LogInformation($"查询环境变量失败：{re}", re.ToJsonStr());
            return;
        }

        var userNameList = re
            .Data.Where(x =>
                x.name.StartsWith(keyBeforeNum) && x.name.EndsWith(userNameKeyAfterNum)
            )
            .ToList();
        QingLongEnv userNameOldEnv = userNameList.FirstOrDefault(x => x.value == userName);

        if (userNameOldEnv == null)
        {
            throw new Exception($"用户名不存在：{userName}");
        }

        var num = userNameOldEnv
            .name.Replace(keyBeforeNum, "")
            .Replace(userNameKeyAfterNum, "")
            .Replace("_", "");
        var key = $"{keyBeforeNum}__{num}__{keyAfterNum}";
        await AddOrUpdateEnvAsync(qingLongApi, key, value, "", logger, cancellationToken);
    }

    /// <summary>
    /// 根据states本身保存
    /// </summary>
    /// <param name="qingLongApi"></param>
    /// <param name="keyBeforeNum"></param>
    /// <param name="value"></param>
    /// <param name="containValue"></param>
    /// <param name="keyAfterNum"></param>
    /// <param name="logger"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task SaveStatesByStatesAsync(
        IQingLongApi qingLongApi,
        string keyBeforeNum,
        string value,
        string containValue,
        string keyAfterNum = "States",
        ILogger logger = null,
        CancellationToken cancellationToken = default
    )
    {
        //查env
        var re = await qingLongApi.GetEnvsAsync(keyBeforeNum);

        if (re.Code != 200)
        {
            logger?.LogInformation($"查询环境变量失败：{re}", re.ToJsonStr());
            return;
        }

        logger?.LogDebug(re.Data.ToJsonStr());

        var statesList = re
            .Data.Where(x => x.name.StartsWith(keyBeforeNum) && x.name.EndsWith(keyAfterNum))
            .ToList();
        QingLongEnv oldEnv = statesList.FirstOrDefault(x => x.value.Contains(containValue));

        if (oldEnv != null)
        {
            logger?.LogInformation("Key已存在，执行更新");
            logger?.LogInformation("Key：{key}", oldEnv.name);

            var update = new UpdateQingLongEnv()
            {
                id = oldEnv.id,
                name = oldEnv.name,
                value = value,
                remarks = string.IsNullOrWhiteSpace(oldEnv.remarks) ? containValue : oldEnv.remarks,
            };

            var updateRe = await qingLongApi.UpdateEnvsAsync(update);
            logger?.LogInformation(updateRe.Code == 200 ? "更新成功！" : updateRe.ToJsonStr());

            return;
        }

        logger?.LogInformation("Key不存在，执行新增");
        var maxNum = -1;
        if (statesList.Any())
        {
            maxNum = statesList
                .Select(x =>
                {
                    var num = x
                        .name.Replace($"{keyBeforeNum}__", "")
                        .Replace($"__{keyAfterNum}", "");
                    var parseSuc = int.TryParse(num, out int envNum);
                    return parseSuc ? envNum : 0;
                })
                .Max();
        }
        var name = $"{keyBeforeNum}__{maxNum + 1}";
        if (!string.IsNullOrWhiteSpace(keyAfterNum))
            name += $"__{keyAfterNum}";
        logger?.LogInformation("Key：{key}", name);

        var add = new AddQingLongEnv()
        {
            name = name,
            value = value,
            remarks = containValue,
        };
        var addRe = await qingLongApi.AddEnvsAsync(new List<AddQingLongEnv> { add });
        logger?.LogInformation(addRe.Code == 200 ? "新增成功！" : addRe.ToJsonStr());
    }
}
