using DiscosWebSdk.Models.EventPayloads;
using DiscosWebSdk.Models.ResponseModels;
using JetBrains.Annotations;

namespace DiscosWebSdk.Interfaces.Clients;

public interface IDiscosClient
{
	[UsedImplicitly]
	public Task<DiscosModelBase?>                 GetSingle(Type      t,  string id, string queryString = "");

	[UsedImplicitly]
	public Task<(DiscosModelBase? Model, DownloadStatus Status)> GetSingleWithDownloadState(Type t, string id, string queryString = "");
	
	[UsedImplicitly]
	public Task<T>                       GetSingle<T>(string id, string queryString = "");
	
	[UsedImplicitly]
	public Task<(T Model, DownloadStatus Status)>                       GetSingleWithDownloadState<T>(string id, string queryString = "");
	
	[UsedImplicitly]
	public Task<IReadOnlyList<DiscosModelBase?>?> GetMultiple(Type t, string queryString = "");
	
	[UsedImplicitly]
	public Task<(IReadOnlyList<DiscosModelBase?>? Models, DownloadStatus Status)> GetMultipleWithDownloadState(Type t, string queryString = "");
	
	[UsedImplicitly]
	public Task<IReadOnlyList<T>>        GetMultiple<T>(string queryString = "");
	
	[UsedImplicitly]
	public Task<(IReadOnlyList<T> Models, DownloadStatus Status)>        GetMultipleWithDownloadState<T>(string queryString = "");
}