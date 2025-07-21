using System;
using System.Threading;

namespace Ray.DDD;

public interface IDataFilter<TFilter>
    where TFilter : class
{
    IDisposable Enable();

    IDisposable Disable();

    bool IsEnabled { get; }
}

public class DataFilter<TFilter> : IDataFilter<TFilter>
    where TFilter : class
{
    public bool IsEnabled
    {
        get
        {
            EnsureInitialized();
            return _filter.Value.IsEnabled;
        }
    }

    private readonly AsyncLocal<DataFilterState> _filter;

    public DataFilter()
    {
        _filter = new AsyncLocal<DataFilterState>();
    }

    public IDisposable Enable()
    {
        EnsureInitialized();
        _filter.Value.IsEnabled = true;

        return new DisposeAction(() => Disable());
    }

    public IDisposable Disable()
    {
        EnsureInitialized();
        _filter.Value.IsEnabled = false;

        return new DisposeAction(() => Enable());
    }

    private void EnsureInitialized()
    {
        if (_filter.Value != null)
            return;

        _filter.Value = new DataFilterState(true);
    }
}
