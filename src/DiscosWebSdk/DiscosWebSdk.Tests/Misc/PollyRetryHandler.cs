using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly.Retry;

namespace DiscosWebSdk.Tests.Misc;

public class PollyRetryHandler : DelegatingHandler
{
	private readonly AsyncRetryPolicy<HttpResponseMessage> _policy;

	public PollyRetryHandler(AsyncRetryPolicy<HttpResponseMessage> policy, HttpMessageHandler innerHandler)
		: base(innerHandler)
	{
		_policy = policy;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => await _policy.ExecuteAsync(() => base.SendAsync(request, cancellationToken));
}
