using System.Net;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Exceptions.DependencyInjection;
using DiscosWebSdk.Interfaces.BulkFetching;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Options;
using DiscosWebSdk.Queries.Builders;
using DiscosWebSdk.Services.BulkFetching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace DiscosWebSdk.DependencyInjection;

public static class DependencyInjectionExtensions
{
	private static readonly List<TimeSpan> DefaultRetrySpans = new[] {1, 2, 5, 10, 30, 60, 60, 60}.Select(i => TimeSpan.FromSeconds(i)).ToList();
	
	public static void AddDiscosServices(this IServiceCollection services, IConfiguration configuration, bool usePolly = false, IEnumerable<TimeSpan>? retrySpans = null)
	{
		IConfigurationSection configSection = configuration.GetSection(nameof(DiscosOptions));
		DiscosOptions         opt           = configSection.Get<DiscosOptions>();

		if (opt.DiscosApiKey is null || opt.DiscosApiUrl is null)
		{
			throw new InvalidDiscosConfigurationException("DISCOS API key and URL not set");
		}

		services.Configure<DiscosOptions>(configSection);

		services.AddTransient(typeof(IDiscosQueryBuilder<>), typeof(DiscosQueryBuilder<>));

		if (usePolly)
		{
			services.AddHttpClient<IDiscosClient, DiscosClient>(c =>
																{
																	c.BaseAddress                         = new(opt.DiscosApiUrl);
																	c.DefaultRequestHeaders.Authorization = new("bearer", opt.DiscosApiKey);
																}).AddTransientHttpErrorPolicy(c => c.OrResult(res => res.StatusCode is HttpStatusCode.TooManyRequests).WaitAndRetryAsync(retrySpans ?? DefaultRetrySpans));
		}
		else
		{
			services.AddHttpClient<IDiscosClient, DiscosClient>(c =>
																{
																	c.BaseAddress                         = new(opt.DiscosApiUrl);
																	c.DefaultRequestHeaders.Authorization = new("bearer", opt.DiscosApiKey);
																});
		}

		services.AddTransient(typeof(IBulkFetchService<>), typeof(ImmediateBulkFetchService<>));

	}
}
