using System.Text;
using DISCOSweb_Sdk.Exceptions.Queries.Filters.FilterTree;
using DISCOSweb_Sdk.Extensions;
using DISCOSweb_Sdk.Interfaces;
using DISCOSweb_Sdk.Queries.Filters;
using DISCOSweb_Sdk.Queries.Filters.FilterTree;

namespace DISCOSweb_Sdk.Queries;

internal class DiscosQueryBuilder<TObject>: IDiscosQueryBuilder<TObject> where TObject : notnull
{
	private FilterTree _filterTree;
	private List<string> _includes;
	private int? _numPages;
	private int? _pageNum;

	public DiscosQueryBuilder()
	{
		_filterTree = new();
		_includes = new();
		Reset();
	}

	public IDiscosQueryBuilder<TObject> AddFilter<TParam>(FilterDefinition<TObject, TParam> filterDefinition)
	{
		_filterTree.AddDefinitionNode(filterDefinition);
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddInclude(string fieldName)
	{
		typeof(TObject).EnsureFieldExists(fieldName);
		_includes.Add(fieldName);
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddAllIncludes() => throw new NotImplementedException();

	public IDiscosQueryBuilder<TObject> AddPageSize(int numPages)
	{
		_numPages = numPages;
		return this;
	}

	public IDiscosQueryBuilder<TObject> PageNum(int pageNum)
	{
		_pageNum = pageNum;
		return this;
	}

	public IDiscosQueryBuilder<TObject> Reset()
	{
		_filterTree = new();
		_includes = new();
		_numPages = null;
		_pageNum = null;
		return this;
	}

	public string Build()
	{
		StringBuilder builder = new();
		if (!_filterTree.IsEmpty)
		{
			AddFilterString(builder);
		}
		if (_includes.Count > 0)
		{
			AddIncludeString(builder);
		}
		if (_numPages is not null)
		{
			AddNumPagesString(builder);
		}
		if (_pageNum is not null)
		{
			AddPageNumberString(builder);
		}
		return builder.ToString();
	}

	private void AddFilterString(StringBuilder builder)
	{
		if (!_filterTree.IsCompleteTree())
		{
			throw new InvalidFilterTreeException("Filter tree is not complete");
		}
		builder.Append("filter=");
		builder.Append(_filterTree);
	}

	private void AddIncludeString(StringBuilder builder)
	{
		builder.Append("include=");
		_includes.ForEach(i => builder.Append(i));
	}

	private void AddPageNumberString(StringBuilder builder)
	{
		builder.Append($"page[number]={_pageNum}");
	}

	private void AddNumPagesString(StringBuilder builder)
	{
		builder.Append($"page[size]={_numPages}");
	}
}
