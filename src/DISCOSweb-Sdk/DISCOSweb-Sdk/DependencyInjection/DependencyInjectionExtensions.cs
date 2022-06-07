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
		//TODO - Add HTTP client
		services.AddTransient(typeof(IDiscosQueryBuilder<>), typeof(DiscosQueryBuilder<>));
		services.Configure<DiscosOptions>(configuration.GetSection(nameof(DiscosOptions)));
	}
}
