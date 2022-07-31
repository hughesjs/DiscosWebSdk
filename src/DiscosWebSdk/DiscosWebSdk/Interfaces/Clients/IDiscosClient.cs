using DiscosWebSdk.Models.ResponseModels;
using JetBrains.Annotations;

namespace DiscosWebSdk.Interfaces.Clients;

public interface IDiscosClient
{
	[UsedImplicitly]
	public Task<DiscosModelBase?> GetSingle(Type t, string id, string queryString = "");
	
	[UsedImplicitly]
	public Task<T> GetSingle<T>(string id, string queryString = "");
	
	[UsedImplicitly]
	public Task<IReadOnlyList<DiscosModelBase?>?> GetMultiple(Type t, string queryString = "");

	[UsedImplicitly]
	public Task<ModelsWithPagination<DiscosModelBase>> GetMultipleWithPaginationState(Type t, string queryString = "");

	[UsedImplicitly]
	public Task<IReadOnlyList<T>> GetMultiple<T>(string queryString = "");

	[UsedImplicitly]
	public Task<ModelsWithPagination<T>> GetMultipleWithPaginationState<T>(string queryString = "") where T: DiscosModelBase;
}
