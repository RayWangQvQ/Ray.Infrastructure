using System;

namespace Ray.DDD;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string errorMessage)
        : base(errorMessage)
    {
    }
}
