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
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Models.Misc;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Services.Queries;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DiscosWebSdk.Clients;

public class DiscosClient : IDiscosClient
{
	private const string SingleFetchTemplate   = "{0}/{1}{2}";
	private const string MultipleFetchTemplate = "{0}{1}";

	private readonly HttpClient            _client;
	private readonly ILogger<DiscosClient> _logger;

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

	private readonly IQueryVerificationService _queryVerificationService;

	public DiscosClient(HttpClient client, ILogger<DiscosClient> logger, IQueryVerificationService queryVerificationService)
	{
		_logger                   = logger;
		_queryVerificationService = queryVerificationService;
		_client                   = client;
	}


	private string GetSingleFetchUrl(string endpoint, string id, string queryString)
	{
		string fetchUrl = string.Format(SingleFetchTemplate, endpoint, id, queryString);
		_logger.LogDebug("Fetching from {FetchUrl}", fetchUrl);
		return fetchUrl;
	}

	private string GetMultipleFetchUrl(string endpoint, string queryString)
	{
		string fetchUrl = string.Format(MultipleFetchTemplate, endpoint, queryString);
		_logger.LogDebug("Fetching from {FetchUrl}", fetchUrl);
		return fetchUrl;
	}

	public async Task<DiscosModelBase?> GetSingle(Type t, string id, string queryString = "")
	{
		_queryVerificationService.CheckQuery(t, queryString);
		_logger.LogInformation("Getting single {Type} with id {Id} and query string {QueryString}", t.Name, id, queryString);
		string              endpoint    = _endpoints[t];
		HttpResponseMessage res         = await _client.GetAsync(GetSingleFetchUrl(endpoint, id, queryString));
		DiscosModelBase?    discosModel = await res.Content.ReadAsJsonApiAsync(t, DiscosObjectResolver.CreateResolver());
		_logger.LogInformation("Successfully fetched object with id {Id}", id);
		return discosModel;
	}

	public async Task<T> GetSingle<T>(string id, string queryString = "")
	{
		_queryVerificationService.CheckQuery<T>(queryString);
		_logger.LogInformation("Getting single {Type} with id {Id} and query string {QueryString}", typeof(T).Name, id, queryString);
		string              endpoint    = _endpoints[typeof(T)];
		HttpResponseMessage res         = await _client.GetAsync(GetSingleFetchUrl(endpoint, id, queryString));
		T                   discosModel = await res.Content.ReadAsJsonApiAsync<T>(DiscosObjectResolver.CreateResolver());
		_logger.LogInformation("Successfully fetched object with id {Id}", id);
		return discosModel;
	}


	public async Task<IReadOnlyList<T>?> GetMultiple<T>(string queryString = "")
	{
		_queryVerificationService.CheckQuery<T>(queryString);
		_logger.LogInformation("Getting multiple {Type} with query string {QueryString}", typeof(T).Name, queryString);
		string              endpoint     = _endpoints[typeof(T)];
		HttpResponseMessage res          = await _client.GetAsync(GetMultipleFetchUrl(endpoint, queryString));
		List<T>?            discosModels = await res.Content.ReadAsJsonApiManyAsync<T>(DiscosObjectResolver.CreateResolver());
		_logger.LogInformation("Successfully fetched {Count} objects", discosModels?.Count ?? 0);
		return discosModels;
	}

	public async Task<ModelsWithPagination<T>> GetMultipleWithPaginationState<T>(string queryString = "") where T : DiscosModelBase
	{
		_queryVerificationService.CheckQuery<T>(queryString);
		_logger.LogInformation("Getting multiple {Type} (+ pagination state) with query string {QueryString}", typeof(T).Name, queryString);
		string              endpoint     = _endpoints[typeof(T)];
		HttpResponseMessage res          = await _client.GetAsync(GetMultipleFetchUrl(endpoint, queryString));
		List<T>             discosModels = await res.Content.ReadAsJsonApiManyAsync<T>(DiscosObjectResolver.CreateResolver()) ?? new List<T>();
		_logger.LogInformation("Successfully fetched {Count} objects", discosModels.Count);

		PaginationDetails pagDetails = await GetPaginationDetails(res) ??
									   new()
									   {
										   CurrentPage = 1,
										   TotalPages  = 1,
										   PageSize    = discosModels.Count
									   };


		return new(discosModels, pagDetails);
	}

	public async Task<IReadOnlyList<DiscosModelBase?>?> GetMultiple(Type t, string queryString = "")
	{
		_queryVerificationService.CheckQuery(t, queryString);
		_logger.LogInformation("Getting multiple {Type} with query string {QueryString}", t.Name, queryString);
		string                           endpoint     = _endpoints[t];
		HttpResponseMessage              res          = await _client.GetAsync(GetMultipleFetchUrl(endpoint, queryString));
		IReadOnlyList<DiscosModelBase?>? discosModels = await res.Content.ReadAsJsonApiManyAsync(t, DiscosObjectResolver.CreateResolver());
		_logger.LogInformation("Successfully fetched {Count} objects", discosModels?.Count ?? 0);
		return discosModels;
	}

	public async Task<ModelsWithPagination<DiscosModelBase>> GetMultipleWithPaginationState(Type t, string queryString = "")
	{
		_queryVerificationService.CheckQuery(t, queryString);
		_logger.LogInformation("Getting multiple {Type} (+ pagination state) with query string {QueryString}", t.Name, queryString);
		string              endpoint = _endpoints[t];
		HttpResponseMessage res      = await _client.GetAsync(GetMultipleFetchUrl(endpoint, queryString));

		IReadOnlyList<DiscosModelBase?> discosModels = await res.Content.ReadAsJsonApiManyAsync(t, DiscosObjectResolver.CreateResolver()) ?? ArraySegment<DiscosModelBase?>.Empty;

		_logger.LogInformation("Successfully fetched {Count} objects", discosModels.Count);

		PaginationDetails pagDetails = await GetPaginationDetails(res) ??
									   new()
									   {
										   CurrentPage = 1,
										   TotalPages  = 1,
										   PageSize    = discosModels.Count
									   };
		return new(discosModels, pagDetails);
	}

	private async Task<PaginationDetails?> GetPaginationDetails(HttpResponseMessage res)
	{
		//TODO - This is inefficient, update Hypermedia lib to allow for deserialisation from string so we don't read twice
		using JsonDocument document = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
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
