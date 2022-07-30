using DiscosWebSdk.Clients;
using DiscosWebSdk.Interfaces.BulkFetching;
using DiscosWebSdk.Interfaces.Clients;
using DiscosWebSdk.Models.EventPayloads;
using DiscosWebSdk.Models.ResponseModels;

namespace DiscosWebSdk.Services.BulkFetching;

internal class ImmediateBulkFetchService: IBulkFetchService
{
	private const int MaxPageSize = 100;
	
	public EventHandler<DownloadStatus>? DownloadStatusChanged { get; set; }
	
	private readonly IDiscosClient _discosClient;
	
	public ImmediateBulkFetchService(IDiscosClient discosClient)
	{
		_discosClient = discosClient;
	}

	public List<T> FetchAll<T>() where T : DiscosModelBase
	{
		List<T> results = new();
		
		Task<IReadOnlyList<T>> results = _discosClient.GetMultiple<T>($"?pageSize={MaxPageSize}");
	}
}