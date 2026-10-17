using Mapster;

namespace Ray.Infrastructure.Helpers;

public class AutoMapperHelper
{
    public static TTarget Map<TSource, TTarget>(TSource source)
    {
        return source.Adapt<TSource, TTarget>();
    }
}
