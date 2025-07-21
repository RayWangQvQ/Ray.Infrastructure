using Microsoft.Extensions.DependencyInjection;

namespace Ray.DDD;

public static class DataFilterExtension
{
    public static IServiceCollection AddDataFilter(this IServiceCollection service)
    {
        service.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        return service;
    }
}
