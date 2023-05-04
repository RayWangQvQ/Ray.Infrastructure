using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Ray.Infrastructure.QingLong
{
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
            else if(File.Exists(Path.Combine(authFile, "config/auth.json")))
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

        public static async Task SaveCookieListItemToQinLongAsync(IQingLongApi qingLongApi, 
            string keyBeforeNum, string value, string containValue, string keyAfterNum="States",
            ILogger logger = null, CancellationToken cancellationToken = default)
        {
            //查env
            var re = await qingLongApi.GetEnvs(keyBeforeNum);

            if (re.Code != 200)
            {
                logger?.LogInformation($"查询环境变量失败：{re}", re.ToJsonStr());
                return;
            }

            logger?.LogDebug(re.Data.ToJsonStr());

            var list = re.Data.Where(x => x.name.StartsWith(keyBeforeNum)).ToList();
            QingLongEnv oldEnv = list.FirstOrDefault(x => x.value.Contains(containValue));

            if (oldEnv != null)
            {
                logger?.LogInformation("Key已存在，执行更新");
                logger?.LogInformation("Key：{key}", oldEnv.name);
                var update = new UpdateQingLongEnv()
                {
                    id = oldEnv.id,
                    name = oldEnv.name,
                    value = value,
                    remarks = string.IsNullOrWhiteSpace(oldEnv.remarks)
                        ? containValue
                        : oldEnv.remarks,
                };

                var updateRe = await qingLongApi.UpdateEnvs(update);
                logger?.LogInformation(updateRe.Code == 200 ? "更新成功！" : updateRe.ToJsonStr());

                return;
            }

            logger?.LogInformation("Key不存在，执行新增");
            var maxNum = -1;
            if (list.Any())
            {
                maxNum = list.Select(x =>
                {
                    var num = x.name.Replace($"{keyBeforeNum}__", "").Replace($"__{keyAfterNum}","");
                    var parseSuc = int.TryParse(num, out int envNum);
                    return parseSuc ? envNum : 0;
                }).Max();
            }
            var name = $"{keyBeforeNum}__{maxNum + 1}";
            if (!string.IsNullOrWhiteSpace(keyAfterNum)) name += $"__{keyAfterNum}";
            logger?.LogInformation("Key：{key}", name);

            var add = new AddQingLongEnv()
            {
                name = name,
                value = value,
                remarks = containValue
            };
            var addRe = await qingLongApi.AddEnvs(new List<AddQingLongEnv> { add });
            logger?.LogInformation(addRe.Code == 200 ? "新增成功！" : addRe.ToJsonStr());
        }
    }
}
