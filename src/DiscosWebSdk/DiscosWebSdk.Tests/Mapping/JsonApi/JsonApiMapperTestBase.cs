using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiscosWebSdk.Clients;
using DiscosWebSdk.Mapping.JsonApi;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Models.ResponseModels.Entities;
using DiscosWebSdk.Models.ResponseModels.FragmentationEvent;
using DiscosWebSdk.Models.ResponseModels.Launches;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using DiscosWebSdk.Models.ResponseModels.Orbits;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using DiscosWebSdk.Models.ResponseModels.Reentries;
using DiscosWebSdk.Tests.Misc;
using Hypermedia.JsonApi.Client;
using JetBrains.Annotations;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Mapping.JsonApi;

public abstract class JsonApiMapperTestBase
{
	private readonly        string                                _apiBase = Environment.GetEnvironmentVariable("DISCOS_API_URL") ?? "https://discosweb.esoc.esa.int/api/";
	private readonly        HttpClient                            _client;
	private static readonly List<TimeSpan>                        RetrySpans  = new[] {1, 2, 5, 10, 30, 60, 60, 60}.Select(i => TimeSpan.FromSeconds(i)).ToList();
	private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions.HandleTransientHttpError().OrResult(res => res.StatusCode is HttpStatusCode.TooManyRequests).WaitAndRetryAsync(RetrySpans);


	protected JsonApiMapperTestBase()
	{
		_client = new(new PollyRetryHandler(RetryPolicy, new HttpClientHandler()));
		_client.Timeout = TimeSpan.FromMinutes(20);
		
		_client.BaseAddress                         = new(_apiBase);
		_client.DefaultRequestHeaders.Authorization = new("bearer", Environment.GetEnvironmentVariable("DISCOS_API_KEY"));
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

	protected async Task<T> FetchSingle<T>(string id, string queryString = "")
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await _client.GetAsync($"{_apiBase}{endpoint}/{id}{queryString}");
		return await res.Content.ReadAsJsonApiAsync<T>(DiscosObjectResolver.CreateResolver());
	}

	protected async Task<IReadOnlyList<T>> FetchMultiple<T>(string queryString = "")
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await _client.GetAsync($"{_apiBase}{endpoint}{queryString}");
		return await res.Content.ReadAsJsonApiManyAsync<T>(DiscosObjectResolver.CreateResolver());
	}

	[UsedImplicitly] [Fact] public abstract Task CanGetSingleWithoutLinks();
	[UsedImplicitly] [Fact] public abstract Task CanGetSingleWithLinks();
	[UsedImplicitly] [Fact] public abstract Task CanGetMultiple();
}
