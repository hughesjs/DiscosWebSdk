using DiscosWebSdk.Queries.Filters;

namespace DiscosWebSdk.Interfaces.Queries;

public interface IDiscosQueryBuilder<TObject> : IDiscosQueryBuilder where TObject : notnull
{
	public     IDiscosQueryBuilder<TObject> AddFilter(FilterDefinition filterDefinition);
	public     IDiscosQueryBuilder<TObject> And();
	public     IDiscosQueryBuilder<TObject> Or();
	public     IDiscosQueryBuilder<TObject> AddInclude(string fieldName);
	public     IDiscosQueryBuilder<TObject> AddAllIncludes();
	public new IDiscosQueryBuilder<TObject> AddPageSize(int numPages);
	public new IDiscosQueryBuilder<TObject> AddPageNum(int  pageNum);
	public new IDiscosQueryBuilder<TObject> Reset();
}

public interface IDiscosQueryBuilder
{
	public IDiscosQueryBuilder AddPageSize(int numPages);
	public IDiscosQueryBuilder AddPageNum(int  pageNum);
	public IDiscosQueryBuilder Reset();
	public string                       Build();
}
