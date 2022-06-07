using DISCOSweb_Sdk.Queries.Filters;

namespace DISCOSweb_Sdk.Interfaces.Queries;

public interface IDiscosQueryBuilder<TObject> where TObject : notnull
{
	public IDiscosQueryBuilder<TObject> AddFilter(FilterDefinition filterDefinition);
	public IDiscosQueryBuilder<TObject> And();
	public IDiscosQueryBuilder<TObject> Or();
	public IDiscosQueryBuilder<TObject> AddInclude(string fieldName);
	public IDiscosQueryBuilder<TObject> AddAllIncludes();
	public IDiscosQueryBuilder<TObject> AddPageSize(int numPages);
	public IDiscosQueryBuilder<TObject> AddPageNum(int pageNum);
	public IDiscosQueryBuilder<TObject> Reset();
	public string Build();
}
