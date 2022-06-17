using DiscosWebSdk.Clients;
using DiscosWebSdk.Exceptions.DependencyInjection;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Options;
using DiscosWebSdk.Queries.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscosWebSdk.DependencyInjection;

public static class DependencyInjectionExtensions
{
	public static void AddDiscosServices(this IServiceCollection services, IConfiguration configuration)
	{
		IConfigurationSection configSection = configuration.GetSection(nameof(DiscosOptions));
		DiscosOptions         opt           = configSection.Get<DiscosOptions>();

		if (opt.DiscosApiKey is null || opt.DiscosApiUrl is null)
		{
			throw new InvalidDiscosConfigurationException("DISCOS API key and URL not set");
		}
		
		services.Configure<DiscosOptions>(configSection);
		
		services.AddTransient(typeof(IDiscosQueryBuilder<>), typeof(DiscosQueryBuilder<>));

		services.AddHttpClient<IDiscosClient, DiscosClient>(c =>
											 {
												 c.BaseAddress                         = new(opt.DiscosApiUrl);
												 c.DefaultRequestHeaders.Authorization = new("bearer", opt.DiscosApiKey);
											 });
	}
}
