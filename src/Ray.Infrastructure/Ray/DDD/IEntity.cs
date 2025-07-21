using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray.DDD
{
    /// <summary>
    /// 无主键实体
    /// </summary>
    public interface IEntity
    {
    }

    /// <summary>
    /// 主键实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}
