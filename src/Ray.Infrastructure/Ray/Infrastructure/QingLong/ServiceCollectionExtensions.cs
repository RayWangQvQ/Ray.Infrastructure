using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Ray.Infrastructure.QingLong
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQingLongRefitApi(this IServiceCollection services, Action<IHttpClientBuilder> builderAction = null)
        {
            var qinglongHost = Environment.GetEnvironmentVariable("QL_URL") ?? "http://localhost:5600";

            var token = "";
            try
            {
                token = $"Bearer {QingLongHelper.GetAuthToken()}";
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "获取token异常");
                Console.WriteLine($"获取token异常：{ex.Message}");
            }

            IHttpClientBuilder builder = services.AddRefitClient<IQingLongApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(qinglongHost);
                    if (!string.IsNullOrWhiteSpace(token))
                        c.DefaultRequestHeaders.Add("Authorization", new List<string> { token });
                });

            builderAction?.Invoke(builder);

            return services;
        }
    }
}
