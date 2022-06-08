using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using DISCOSweb_Sdk.Clients;
using DISCOSweb_Sdk.DependencyInjection;
using DISCOSweb_Sdk.Interfaces.Queries;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.DependencyInjection;

public class DependencyInjectionExtensionsTests
{
	private const    string             ApiKey = "1234";
	private const    string             ApiUrl = "http://my.api.local/";

	private readonly Dictionary<string, string> _configDict = new()
															  {
																  {"DiscosOptions:DiscosApiKey", ApiKey},
																  {"DiscosOptions:DiscosApiUrl", ApiUrl}
															  };
	private readonly IServiceProvider _provider;
	
	public DependencyInjectionExtensionsTests()
	{
		ServiceCollection     services     = new();

		ConfigurationBuilder builder = new();
		IConfigurationRoot?  config  = builder.AddInMemoryCollection(_configDict).Build();
		
		services.AddDiscosServices(config);
		
		_provider = services.BuildServiceProvider();
	}
	
	
	[Fact]
	public void CanGetQueryBuilderFromContainer()
	{
		IDiscosQueryBuilder<DiscosObject>? builder = _provider.GetService<IDiscosQueryBuilder<DiscosObject>>();
		builder.ShouldNotBeNull();
	}

	[Fact]
	public void CanGetDiscosClientFromContainer()
	{
		IDiscosClient? client = _provider.GetService<IDiscosClient>();
		HttpClient internalClient = (HttpClient)typeof(DiscosClient).GetField("_client", BindingFlags.Instance | BindingFlags.NonPublic)!
																	.GetValue(client)!;
		
		client.ShouldNotBeNull();
		internalClient.BaseAddress.ShouldBe(new(ApiUrl));
		internalClient.DefaultRequestHeaders.Authorization!.Scheme.ShouldBe("bearer");
		internalClient.DefaultRequestHeaders.Authorization.Parameter.ShouldBe(ApiKey);
	}

	[Fact]
	public void CanGetDiscosOptionsFromContainer()
	{
		IOptions<DiscosOptions>? options = _provider.GetService<IOptions<DiscosOptions>>();
		options.ShouldNotBeNull();
		options.Value.DiscosApiKey.ShouldBe(ApiKey);
		options.Value.DiscosApiUrl.ShouldBe(ApiUrl);
	}
}
