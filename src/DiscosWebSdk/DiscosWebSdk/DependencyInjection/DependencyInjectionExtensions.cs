using System.Net;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Exceptions.DependencyInjection;
using DiscosWebSdk.Interfaces.BulkFetching;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Options;
using DiscosWebSdk.Queries.Builders;
using DiscosWebSdk.Services.BulkFetching;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace DiscosWebSdk.DependencyInjection;

public static class DependencyInjectionExtensions
{
	private static readonly List<TimeSpan> DefaultRetrySpans = new[] {1, 2, 5, 10, 30, 60, 60, 60}.Select(i => TimeSpan.FromSeconds(i)).ToList();
	
	public static void AddDiscosServices(this IServiceCollection services, IConfiguration configuration, bool usePolly = false, IEnumerable<TimeSpan>? retrySpans = null) => services.RegisterEverything(configuration, usePolly, retrySpans);
	
	[UsedImplicitly]
	public static void AddDiscosServices(this IServiceCollection services, string apiUrl, string apiKey, bool usePolly = false, IEnumerable<TimeSpan>? retrySpans = null) => services.RegisterEverything(BuildConfiguration(apiUrl, apiKey), usePolly, retrySpans);

	[UsedImplicitly]
	private static IServiceCollection RegisterEverything(this IServiceCollection services, IConfiguration configuration, bool usePolly = false, IEnumerable<TimeSpan>? retrySpans = null)
	{
		IConfigurationSection configSection = configuration.GetSection(nameof(DiscosOptions));
		DiscosOptions         opt           = configSection.Get<DiscosOptions>();

		if (opt.DiscosApiKey is null || opt.DiscosApiUrl is null)
		{
			throw new InvalidDiscosConfigurationException("DISCOS API key and URL not set");
		}

		services.Configure<DiscosOptions>(configSection);

		services.AddTransient(typeof(IDiscosQueryBuilder<>), typeof(DiscosQueryBuilder<>));
		services.AddTransient(typeof(IBulkFetchService<>),   typeof(ImmediateBulkFetchService<>));

		services.AddTransient<IDiscosQueryBuilder, DiscosQueryBuilder>();
		services.AddTransient<IBulkFetchService, ImmediateBulkFetchService>();

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
		return services;
	}

	private static IConfiguration BuildConfiguration(string apiUrl, string apiKey)
	{
		if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiKey))
		{
			throw new InvalidDiscosConfigurationException("DISCOS API key and URL not set");
		}

		IConfigurationBuilder builder = new ConfigurationBuilder()
		   .AddInMemoryCollection(new Dictionary<string, string>
								  {
									  { "DiscosOptions:DiscosApiKey", apiUrl},
									  { "DiscosOptions:DiscosApiUrl", apiKey}
								  });

		return builder.Build();
	}
}
