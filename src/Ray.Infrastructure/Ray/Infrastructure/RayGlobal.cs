using System;

namespace Ray.Infrastructure
{
    public class RayGlobal
    {
        /// <summary>
        /// 根容器
        /// </summary>
        public static IServiceProvider ServiceProviderRoot { get; set; }
    }
}
