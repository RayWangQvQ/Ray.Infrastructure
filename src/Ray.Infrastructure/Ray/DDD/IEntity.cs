namespace Ray.DDD;

/// <summary>
/// 无主键实体
/// </summary>
public interface IEntity { }

/// <summary>
/// 主键实体
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}