using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using DiscosWebSdk.Clients;
using DiscosWebSdk.DependencyInjection;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Options;
using DiscosWebSdk.Queries.Builders;
using DiscosWebSdk.Tests.TestDataGenerators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.DependencyInjection;

public class DependencyInjectionExtensionsTests
{
	private const string ApiKey = "1234";
	private const string ApiUrl = "http://my.api.local/";

	private readonly Dictionary<string, string> _configDict = new()
															  {
																  {"DiscosOptions:DiscosApiKey", ApiKey},
																  {"DiscosOptions:DiscosApiUrl", ApiUrl}
															  };
	private readonly IServiceProvider _provider;

	public DependencyInjectionExtensionsTests()
	{
		ServiceCollection services = new();

		ConfigurationBuilder builder = new();
		IConfigurationRoot?  config  = builder.AddInMemoryCollection(_configDict).Build();

		services.AddDiscosServices(config);

		_provider = services.BuildServiceProvider();
	}


	[Theory]
	[ClassData(typeof(DiscosModelTypesTestDataGenerator))]
	public void CanGetQueryBuilderFromContainer(Type t, string _)
	{
		Type       builderIType = typeof(IDiscosQueryBuilder<>).MakeGenericType(t);
		Type       builderCType = typeof(DiscosQueryBuilder<>).MakeGenericType(t);

		object builder = _provider.GetService(builderIType)!;
		builder.ShouldBeOfType(builderCType);
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
