using System.Threading;
using System.Threading.Tasks;

namespace Ray.Infrastructure.AutoTask;

public interface IAutoTaskService
{
    Task DoAsync(CancellationToken cancellationToken = default);
}
