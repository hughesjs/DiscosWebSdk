using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiscosWebSdk.Mapping.JsonApi;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.FragmentationEvent;
using DiscosWebSdk.Models.ResponseModels.Launches;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using DiscosWebSdk.Models.ResponseModels.Orbits;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using DiscosWebSdk.Models.ResponseModels.Reentries;
using Hypermedia.JsonApi.Client;
using JetBrains.Annotations;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi;

public abstract class JsonApiMapperTestBase
{
	private readonly string _apiBase = Environment.GetEnvironmentVariable("DISCOS_API_URL") ?? "https://discosweb.esoc.esa.int/api/";

	private readonly Dictionary<Type, string> _endpoints = new()
														   {
															   {typeof(DiscosObject), "objects"},
															   {typeof(DiscosObjectClass), "object-classes"},
															   {typeof(Country), "entities"},
															   {typeof(Organisation), "entities"},
															   {typeof(Entity), "entities"},
															   {typeof(FragmentationEvent), "fragmentations"},
															   {typeof(Launch), "launches"},
															   {typeof(LaunchSite), "launch-sites"},
															   {typeof(LaunchSystem), "launch-systems"},
															   {typeof(LaunchVehicle), "launch-vehicles"},
															   {typeof(LaunchVehicleFamily), "launch-vehicles/families"},
															   {typeof(LaunchVehicleEngine), "launch-vehicles/engines"},
															   {typeof(LaunchVehicleStage), "launch-vehicles/stages"},
															   {typeof(InitialOrbitDetails), "initial-orbits"},
															   {typeof(DestinationOrbitDetails), "destination-orbits"},
															   {typeof(Propellant), "propellants"},
															   {typeof(Reentry), "reentries"}
														   };

	protected async Task<T> FetchSingle<T>(string id, string queryString = "")
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await GetWithRateLimitRetry($"{_apiBase}{endpoint}/{id}{queryString}");
		return await res.Content.ReadAsJsonApiAsync<T>(DiscosObjectResolver.CreateResolver());
	}

	protected async Task<IReadOnlyList<T>> FetchMultiple<T>(string queryString = "")
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await GetWithRateLimitRetry($"{_apiBase}{endpoint}{queryString}");
		return await res.Content.ReadAsJsonApiManyAsync<T>(DiscosObjectResolver.CreateResolver());
	}

	private async Task<HttpResponseMessage> GetWithRateLimitRetry(string uri, int retries = 0)
	{
		const int  maxAttempts = 5;
		HttpClient client      = new();
		client.DefaultRequestHeaders.Authorization = new("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
		HttpResponseMessage res = await client.GetAsync(uri);
		if (res.StatusCode == HttpStatusCode.TooManyRequests)
		{
			retries.ShouldBeLessThan(maxAttempts);
			RetryConditionHeaderValue? retryAfter = res.Headers.RetryAfter;
			Console.WriteLine($"Hit rate limit. Waiting for {retryAfter.Delta.Value.TotalSeconds}s. Retry Number: {retries}");
			await Task.Delay(retryAfter.Delta.Value);
			return await GetWithRateLimitRetry(uri, ++retries);
		}
		if (res.StatusCode == HttpStatusCode.BadGateway)
		{
			retries.ShouldBeLessThan(maxAttempts);
			Console.WriteLine("Bad Gateway, probably transient. Waiting for 5s...");
			await Task.Delay(TimeSpan.FromSeconds(5));
			return await GetWithRateLimitRetry(uri, ++retries);
		}
		res.EnsureSuccessStatusCode();
		return res;
	}

	[UsedImplicitly] [Fact] public abstract Task CanGetSingleWithoutLinks();
	[UsedImplicitly] [Fact] public abstract Task CanGetSingleWithLinks();
	[UsedImplicitly] [Fact] public abstract Task CanGetMultiple();
}
