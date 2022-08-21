using DiscosWebSdk.Models.EventPayloads;
using DiscosWebSdk.Models.ResponseModels;

namespace DiscosWebSdk.Interfaces.BulkFetching;

public interface IBulkFetchService<T> : IBulkFetchService  where T : DiscosModelBase
{
	public Task<List<T>> GetAll(bool includeLinks = false);
}

public interface IBulkFetchService
{
	public EventHandler<DownloadStatus>? DownloadStatusChanged { get; set; }
	public Task<List<DiscosModelBase>> GetAll(Type t, bool includeLinks = false);
}
