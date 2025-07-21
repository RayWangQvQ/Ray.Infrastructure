using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ray.Infrastructure.AutoTask;

namespace Ray.Infrastructure.Http;

public class CookieHttpClientHandler<TTargetAccountInfo> : HttpClientHandler
    where TTargetAccountInfo : TargetAccountInfo
{
    private readonly ILogger<CookieHttpClientHandler<TTargetAccountInfo>> _logger;
    private readonly TargetAccountManager<TTargetAccountInfo> _ckManager;

    public CookieHttpClientHandler(
        ILogger<CookieHttpClientHandler<TTargetAccountInfo>> logger,
        TargetAccountManager<TTargetAccountInfo> ckManager
    )
    {
        _logger = logger;
        _ckManager = ckManager;

        this.UseCookies = true;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        _ckManager.ReplaceCookieContainerWithCurrentAccount(this.CookieContainer);

        HttpResponseMessage re = await base.SendAsync(request, cancellationToken);

        _ckManager.UpdateCurrentCookieContainer(this.CookieContainer);

        return re;
    }
}
