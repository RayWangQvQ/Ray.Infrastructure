namespace Ray.DDD;

public class DataFilterState
{
    public bool IsEnabled { get; set; }

    public DataFilterState(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
}
