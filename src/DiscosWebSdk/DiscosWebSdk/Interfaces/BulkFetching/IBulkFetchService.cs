using DiscosWebSdk.Models.EventPayloads;
using DiscosWebSdk.Models.ResponseModels;

namespace DiscosWebSdk.Interfaces.BulkFetching;

public interface IBulkFetchService
{
	public EventHandler<DownloadStatus>? DownloadStatusChanged { get; set; }

	public List<T> FetchAll<T>() where T : DiscosModelBase;
}