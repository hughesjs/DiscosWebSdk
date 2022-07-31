using System.Text.Json;
using System.Text.Json.Serialization;
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
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Models.Misc;
using DiscosWebSdk.Models.ResponseModels;
using JetBrains.Annotations;

namespace DiscosWebSdk.Clients;

public class DiscosClient : IDiscosClient
{
	private readonly HttpClient _client;

	private const string SingleFetchTemplate = "{0}/{1}{2}";
	private const string MultipleFetchTemplate = "{0}{1}";

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

	private static string GetSingleFetchUrl(string   endpoint, string id, string queryString) => string.Format(SingleFetchTemplate, endpoint, id, queryString);
	private static string GetMultipleFetchUrl(string endpoint, string queryString) => string.Format(MultipleFetchTemplate, endpoint, queryString);
	
	public async Task<DiscosModelBase?> GetSingle(Type t, string id, string queryString = "")
	{
		string              endpoint = _endpoints[t];
		HttpResponseMessage res      = await _client.GetAsync(GetSingleFetchUrl(endpoint, id, queryString));
		return await res.Content.ReadAsJsonApiAsync(t, DiscosObjectResolver.CreateResolver());
	}

	public async Task<T> GetSingle<T>(string id, string queryString = "")
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await _client.GetAsync(GetSingleFetchUrl(endpoint, id, queryString));
		return await res.Content.ReadAsJsonApiAsync<T>(DiscosObjectResolver.CreateResolver());
	}


	public async Task<IReadOnlyList<T>> GetMultiple<T>(string queryString = "")
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await _client.GetAsync(GetMultipleFetchUrl(endpoint, queryString));
		return await res.Content.ReadAsJsonApiManyAsync<T>(DiscosObjectResolver.CreateResolver());
	}

	public async Task<ModelsWithPagination<T>> GetMultipleWithPaginationState<T>(string queryString = "") where T: DiscosModelBase
	{
		string              endpoint = _endpoints[typeof(T)];
		HttpResponseMessage res      = await _client.GetAsync(GetMultipleFetchUrl(endpoint, queryString));
		List<T>             model    = await res.Content.ReadAsJsonApiManyAsync<T>(DiscosObjectResolver.CreateResolver()) ?? new List<T>();
		
		PaginationDetails pagDetails = await GetPaginationDetails(res) ?? new()
																		  {
																			  CurrentPage = 1,
																			  TotalPages  = 1,
																			  PageSize    = model.Count
																		  };
		
		
		return new(model,pagDetails);
	}

	public async Task<IReadOnlyList<DiscosModelBase?>?> GetMultiple(Type t, string queryString = "")
	{
		string              endpoint = _endpoints[t];
		HttpResponseMessage res      = await _client.GetAsync(GetMultipleFetchUrl(endpoint, queryString));
		return await res.Content.ReadAsJsonApiManyAsync(t, DiscosObjectResolver.CreateResolver());
	}
	
	public async Task<ModelsWithPagination<DiscosModelBase>> GetMultipleWithPaginationState(Type t, string queryString = "")
	{
		string                           endpoint = _endpoints[t];
		HttpResponseMessage              res      = await _client.GetAsync(GetMultipleFetchUrl(endpoint, queryString));
		
		IReadOnlyList<DiscosModelBase?> model =  await res.Content.ReadAsJsonApiManyAsync(t, DiscosObjectResolver.CreateResolver()) ?? ArraySegment<DiscosModelBase?>.Empty;
		
		PaginationDetails pagDetails = await GetPaginationDetails(res) ?? new()
																		  {
																			  CurrentPage = 1,
																			  TotalPages  = 1,
																			  PageSize    = model.Count
																		  };
		return new(model,pagDetails);
	}

	private async Task<PaginationDetails?> GetPaginationDetails(HttpResponseMessage res)
	{
		//TODO - This is inefficient, update Hypermedia lib to allow for deserialisation from string so we don't read twice
		using JsonDocument document   = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
		return document.RootElement.Deserialize<Response>()?.Meta?.PaginationDetails;
	}
	
	// Need these to deserialise the pagination data...
	[UsedImplicitly]
	private class Response
	{
		[JsonPropertyName("meta")]
		public Meta? Meta { get; [UsedImplicitly] init; }
	}
	[UsedImplicitly]
	private class Meta
	{
		[JsonPropertyName("pagination")]
		public PaginationDetails? PaginationDetails { get; [UsedImplicitly] init; }
	}
}
