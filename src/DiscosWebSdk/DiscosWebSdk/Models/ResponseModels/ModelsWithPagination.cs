using DiscosWebSdk.Models.Misc;

namespace DiscosWebSdk.Models.ResponseModels;

public class ModelsWithPagination<T> : ModelsWithPagination where T: DiscosModelBase
{
	public ModelsWithPagination(IReadOnlyList<T?> models, PaginationDetails paginationDetails) : base(models, paginationDetails)
	{
		Models            = models.Where(m => m is not null).Cast<T>().ToList();
	}
	
	public override IReadOnlyList<T> Models { get; }
}

public class ModelsWithPagination
{
	public ModelsWithPagination(IReadOnlyList<DiscosModelBase?> models, PaginationDetails paginationDetails)
	{
		Models            = models.Where(m => m is not null).Cast<DiscosModelBase>().ToList();
		PaginationDetails = paginationDetails;
	}
	
	public virtual IReadOnlyList<DiscosModelBase> Models { get; }
	public PaginationDetails PaginationDetails { get; }
}
