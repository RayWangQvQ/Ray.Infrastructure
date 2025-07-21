using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray.DDD
{
    public static class DataFilterExtension
    {
        public static IServiceCollection AddDataFilter(this IServiceCollection service)
        {
            service.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
            return service;
        }
    }
}
