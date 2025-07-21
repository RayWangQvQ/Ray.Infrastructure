using System;

namespace Ray.Infrastructure.Extensions.MsDi
{
    public struct ServiceCacheKeyDto
    {
        public Type Type { get; set; }
        public int Slot { get; set; }
    }
}
