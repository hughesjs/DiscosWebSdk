using System.Net;
using System.Net.Http.Headers;
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
using DiscosWebSdk.Extensions;
using DiscosWebSdk.Models.ResponseModels;
using JetBrains.Annotations;

namespace DiscosWebSdk.Clients;

public class DiscosClient : IDiscosClient
{
	private readonly HttpClient _client;

	public DiscosClient(HttpClient client)
	{
		_client = client;
	}


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

	public async Task<DiscosModelBase?> GetSingle(Type t, string id, string queryString = "")
	{
		string              endpoint = _endpoints[t];
		HttpResponseMessage res      = await GetWithRateLimitRetry($"{endpoint}/{id}{queryString}");
		return await res.Content.ReadAsJsonApiAsync(t, DiscosObjectResolver.CreateResolver());
	}


	public async Task<T> GetSingle<T>(string id, string queryString = "")
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await GetWithRateLimitRetry($"{endpoint}/{id}{queryString}");
		return await res.Content.ReadAsJsonApiAsync<T>(DiscosObjectResolver.CreateResolver());
	}

	public async Task<IReadOnlyList<T>> GetMultiple<T>(string queryString = "")
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await GetWithRateLimitRetry($"{endpoint}{queryString}");
		return await res.Content.ReadAsJsonApiManyAsync<T>(DiscosObjectResolver.CreateResolver());
	}

	public async Task<IReadOnlyList<DiscosModelBase?>?> GetMultiple(Type t, string queryString = "")
	{
		string              endpoint = _endpoints[t];
		HttpResponseMessage res      = await GetWithRateLimitRetry($"{endpoint}{queryString}");
		return await res.Content.ReadAsJsonApiManyAsync(t, DiscosObjectResolver.CreateResolver());
	}

	private async Task<HttpResponseMessage> GetWithRateLimitRetry(string uri, int retries = 0)
	{
		const int           maxAttempts = 5;
		HttpResponseMessage res         = await _client.GetAsync(uri);
		if (res.StatusCode == HttpStatusCode.TooManyRequests)
		{
			if (retries >= maxAttempts) return res;
			RetryConditionHeaderValue? retryAfter = res.Headers.RetryAfter;
			Console.WriteLine($"Hit rate limit. Waiting for {retryAfter.Delta.Value.TotalSeconds}s. Retry Number: {retries}");
			await Task.Delay(retryAfter.Delta.Value);
			return await GetWithRateLimitRetry(uri, ++retries);
		}
		if (res.StatusCode == HttpStatusCode.BadGateway)
		{
			if (retries >= maxAttempts) return res;
			Console.WriteLine("Bad Gateway, probably transient. Waiting for 5s...");
			await Task.Delay(TimeSpan.FromSeconds(5));
			return await GetWithRateLimitRetry(uri, ++retries);
		}
		res.EnsureSuccessStatusCode();
		return res;
	}
}


public interface IDiscosClient
{
	[UsedImplicitly]
	public Task<DiscosModelBase?>                 GetSingle(Type      t,  string id, string queryString = "");
	
	[UsedImplicitly]
	public Task<T>                       GetSingle<T>(string id, string queryString = "");
	
	[UsedImplicitly]
	public Task<IReadOnlyList<DiscosModelBase?>?> GetMultiple(Type t, string queryString = "");
	
	[UsedImplicitly]
	public Task<IReadOnlyList<T>>        GetMultiple<T>(string queryString = "");
}
