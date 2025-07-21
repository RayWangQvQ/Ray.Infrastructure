namespace Ray.Infrastructure.Extensions.Json;

public class FilterPropsOption
{
    public FilterEnum FilterEnum { get; set; } = FilterEnum.Ignore;

    public string[] Props { get; set; } = { };
}
