using DiscosWebSdk.Exceptions;
using DiscosWebSdk.Extensions;
using DiscosWebSdk.Interfaces.BulkFetching;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Models.EventPayloads;
using DiscosWebSdk.Models.ResponseModels;
using Microsoft.Extensions.Logging;

namespace DiscosWebSdk.Services.BulkFetching;

// NOTE: This class assumes that you're using something else to handle rate-limiting on the HTTP-Client
// The recommended (and default) way to do this is with Polly (see the DI Extensions)
internal class ImmediateBulkFetchService<T>: ImmediateBulkFetchService, IBulkFetchService<T> where T: DiscosModelBase
{
	public ImmediateBulkFetchService(IDiscosClient discosClient, IDiscosQueryBuilder queryBuilder, ILogger<ImmediateBulkFetchService<T>> logger): base(discosClient, queryBuilder, logger) { }

	public async Task<List<T>> GetAll(bool includeLinks = false)
	{
		List<DiscosModelBase> res = await base.GetAll(typeof(T), includeLinks);
		return res.Cast<T>().ToList();
	}
}


internal class ImmediateBulkFetchService : IBulkFetchService
{
	private const int MaxPageSize = 100;
	
	public EventHandler<DownloadStatus>? DownloadStatusChanged { get; set; }


	private readonly IDiscosClient       _discosClient;
	private readonly IDiscosQueryBuilder _queryBuilder;
	private readonly ILogger<ImmediateBulkFetchService> _logger;

	public ImmediateBulkFetchService(IDiscosClient discosClient, IDiscosQueryBuilder queryBuilder, ILogger<ImmediateBulkFetchService> logger)
	{
		_logger = logger;
		_discosClient = discosClient;
		_queryBuilder = queryBuilder;
	}

	public async Task<List<DiscosModelBase>> GetAll(Type t, bool includeLinks = false)
	{
		_logger.LogInformation("Beginning to fetch all {ObjectType} from DISCOSweb", t.Name);
		List<DiscosModelBase>                 allResults = new();
		int                                   pageNum    = 1;
		ModelsWithPagination<DiscosModelBase> res;
		while ((res = await _discosClient.GetMultipleWithPaginationState(t, GetQueryString(pageNum++, includeLinks, t))).Models.Count > 0)
		{
			allResults.AddRange(res.Models);
			int totalDownloadedSoFar = (res.PaginationDetails.CurrentPage - 1) * res.PaginationDetails.PageSize;
			int totalNumberToFetch = res.PaginationDetails.TotalPages * res.PaginationDetails.PageSize;
			_logger.LogDebug("Downloaded {Downloaded} of {Total} {ObjectType}", totalDownloadedSoFar, totalNumberToFetch, t.Name);
			DownloadStatusChanged?.Invoke(this, new()
												{
													Downloaded = totalDownloadedSoFar,
													Total      = totalNumberToFetch
												});
		}
		return allResults;
	}
	
	private string GetQueryString(int pageNum, bool includeLinks, Type t)
	{
		if (includeLinks)
		{
			if (!t.IsDiscosModel())
			{
				throw new NotDiscosTypeException(t);
			}
			_queryBuilder.AddAllIncludes(t);
		}
		
		_queryBuilder.AddPageNum(pageNum);
		_queryBuilder.AddPageSize(MaxPageSize);
		string queryString = _queryBuilder.Build();
		_logger.LogDebug("Query-String for Bulk-Fetch: {QueryString}", queryString);
		return queryString;
	}
}
