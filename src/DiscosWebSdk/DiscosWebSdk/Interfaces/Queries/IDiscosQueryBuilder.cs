using DiscosWebSdk.Queries.Filters;

namespace DiscosWebSdk.Interfaces.Queries;

public interface IDiscosQueryBuilder<TObject> : IDiscosQueryBuilder where TObject : notnull
{
	public new IDiscosQueryBuilder<TObject> And();
	public new IDiscosQueryBuilder<TObject> Or();
	public new IDiscosQueryBuilder<TObject> AddPageSize(int numPages);
	public new IDiscosQueryBuilder<TObject> AddPageNum(int  pageNum);
	public new IDiscosQueryBuilder<TObject> Reset();
	public IDiscosQueryBuilder<TObject> AddInclude(string fieldName);
	public IDiscosQueryBuilder<TObject> AddAllIncludes();
	public IDiscosQueryBuilder<TObject> AddFilter(FilterDefinition filterDefinition);
}

public interface IDiscosQueryBuilder
{
	public IDiscosQueryBuilder AddFilter(Type t, FilterDefinition filterDefinition);
	public IDiscosQueryBuilder And();
	public IDiscosQueryBuilder Or();
	public IDiscosQueryBuilder AddPageSize(int numPages);
	public IDiscosQueryBuilder AddPageNum(int  pageNum);
	public IDiscosQueryBuilder AddAllIncludes(Type t);
	public IDiscosQueryBuilder AddInclude(Type t, string fieldName);
	public IDiscosQueryBuilder Reset();
	public string Build();
}
