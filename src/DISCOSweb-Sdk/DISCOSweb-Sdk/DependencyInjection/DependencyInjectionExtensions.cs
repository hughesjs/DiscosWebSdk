using DISCOSweb_Sdk.Clients;
using DISCOSweb_Sdk.Exceptions.DependencyInjection;
using DISCOSweb_Sdk.Interfaces.Queries;
using DISCOSweb_Sdk.Options;
using DISCOSweb_Sdk.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace DISCOSweb_Sdk.DependencyInjection;

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

		services.AddHttpClient<DiscosClient>(c =>
											 {
												 c.BaseAddress                         = new(opt.DiscosApiUrl);
												 c.DefaultRequestHeaders.Authorization = new("bearer", opt.DiscosApiKey);
											 });
		
	}
}
