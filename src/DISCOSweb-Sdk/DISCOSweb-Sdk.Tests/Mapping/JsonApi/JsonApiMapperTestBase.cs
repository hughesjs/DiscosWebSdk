using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DISCOSweb_Sdk.Mapping.JsonApi;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Entities;
using DISCOSweb_Sdk.Models.ResponseModels.FragmentationEvent;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;
using Hypermedia.JsonApi.Client;
using JetBrains.Annotations;
using Shouldly;

namespace DISCOSweb_Sdk.Tests.Mapping.JsonApi;

public abstract class JsonApiMapperTestBase
{
	private const string ApiBase = "https://discosweb.esoc.esa.int/api/";

	private readonly Dictionary<Type, string> _endpoints = new()
														   {
															   {typeof(DiscosObject), "objects"},
															   {typeof(DiscosObjectClass), "object-classes"},
															   {typeof(Country), "entities"},
															   {typeof(Organisation), "entities"},
															   {typeof(Entity), "entities"},
															   {typeof(FragmentationEvent), "fragmentations"},
															   {typeof(Launch), "launches"},
															   {typeof(LaunchSite), "launch-sites"}
														   };

	protected async Task<T> FetchSingle<T>(string id, string queryString = "")
	{
		string endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res = await GetWithRateLimitRetry($"{ApiBase}{endpoint}/{id}{queryString}");
		return await res.Content.ReadAsJsonApiAsync<T>(DiscosObjectResolver.CreateResolver());
	}

	protected async Task<IReadOnlyList<T>> FetchMultiple<T>(string queryString = "")
	{
		string endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res = await GetWithRateLimitRetry($"{ApiBase}{endpoint}{queryString}");
		return await res.Content.ReadAsJsonApiManyAsync<T>(DiscosObjectResolver.CreateResolver());
	}
	
	private async Task<HttpResponseMessage> GetWithRateLimitRetry(string uri, int retries = 0)
	{
		const int maxAttempts = 5;
		HttpClient client = new();
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
		res.EnsureSuccessStatusCode();
		return res;
	}

	[UsedImplicitly] public abstract Task CanGetSingleWithoutLinks();
	[UsedImplicitly] public abstract Task CanGetSingleWithLinks();
	[UsedImplicitly] public abstract Task CanGetMultiple();
}
