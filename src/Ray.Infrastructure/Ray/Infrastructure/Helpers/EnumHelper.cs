using System;

namespace Ray.Infrastructure.Helpers;

public static class EnumHelper
{
    public static T[] AsArray<T>()
        where T : Enum
    {
        return (T[])Enum.GetValues(typeof(T));
    }
}
