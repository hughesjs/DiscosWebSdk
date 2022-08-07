using DiscosWebSdk.Interfaces.BulkFetching;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Models.EventPayloads;
using DiscosWebSdk.Models.ResponseModels;

namespace DiscosWebSdk.Services.BulkFetching;

// NOTE: This class assumes that you're using something else to handle rate-limiting on the HTTP-Client
// The recommended way to do this is with Polly (see the DI Extensions)
internal class ImmediateBulkFetchService<T>: ImmediateBulkFetchService, IBulkFetchService<T> where T: DiscosModelBase
{
	public ImmediateBulkFetchService(IDiscosClient discosClient, IDiscosQueryBuilder queryBuilder): base(discosClient, queryBuilder, typeof(T))
	{

	}

	public new async Task<List<T>> GetAll()
	{
		List<DiscosModelBase> res = await base.GetAll();
		return res.Cast<T>().ToList();
	}
}


internal class ImmediateBulkFetchService : IBulkFetchService
{
	private const int MaxPageSize = 100;
	
	public       EventHandler<DownloadStatus>? DownloadStatusChanged { get; set; }


	private readonly IDiscosClient       _discosClient;
	private readonly IDiscosQueryBuilder _queryBuilder;
	private readonly Type                _t;

	public ImmediateBulkFetchService(IDiscosClient discosClient, IDiscosQueryBuilder queryBuilder, Type t)
	{
		_discosClient = discosClient;
		_queryBuilder = queryBuilder;
		_t      = t;
	}

	public async Task<List<DiscosModelBase>> GetAll()
	{
		List<DiscosModelBase>                 allResults = new();
		int                                   pageNum    = 1;
		ModelsWithPagination<DiscosModelBase> res;
		while ((res = await _discosClient.GetMultipleWithPaginationState(_t, GetQueryString(pageNum++))).Models.Count > 0)
		{
			allResults.AddRange(res.Models);
			DownloadStatusChanged?.Invoke(this, new()
												{
													Downloaded = (res.PaginationDetails.CurrentPage - 1) * res.PaginationDetails.PageSize,
													Total      = res.PaginationDetails.TotalPages* res.PaginationDetails.PageSize
												});
		}
		return allResults;
	}
	
	private string GetQueryString(int pageNum)
	{
		_queryBuilder.AddPageNum(pageNum);
		_queryBuilder.AddPageSize(MaxPageSize);
		return _queryBuilder.Build();
	}
}
