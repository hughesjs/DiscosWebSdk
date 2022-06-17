using System.Reflection;
using System.Text;
using DiscosWebSdk.Enums;
using DiscosWebSdk.Exceptions.Queries.Filters.FilterTree;
using DiscosWebSdk.Extensions;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Queries.Filters;
using DiscosWebSdk.Queries.Filters.FilterTree;

namespace DiscosWebSdk.Queries.Builders;

internal class DiscosQueryBuilder<TObject> : IDiscosQueryBuilder<TObject>
{
	private FilterTree   _filterTree;
	private List<string> _includes;
	private int?         _numPages;
	private int?         _pageNum;

	public DiscosQueryBuilder()
	{
		_filterTree = new();
		_includes   = new();
		Reset();
	}

	public IDiscosQueryBuilder<TObject> AddFilter(FilterDefinition filterDefinition)
	{
		if (filterDefinition.GetType().GetGenericArguments().First() != typeof(TObject))
		{
			throw new InvalidFilterTreeException("Filter object type must match query builder type");
		}
		_filterTree.AddDefinitionNode(filterDefinition);
		return this;
	}

	public IDiscosQueryBuilder<TObject> And()
	{
		_filterTree.AddOperationNode(FilterOperation.And);
		return this;
	}


	public IDiscosQueryBuilder<TObject> Or()
	{
		_filterTree.AddOperationNode(FilterOperation.Or);
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddInclude(string fieldName)
	{
		typeof(TObject).EnsureFieldExists(fieldName);
		_includes.Add(AttributeUtilities.GetJsonPropertyName<TObject>(fieldName));
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddAllIncludes()
	{
		IEnumerable<PropertyInfo> props = typeof(TObject).GetProperties()
														 .Where(p => p.PropertyType.IsAssignableTo(typeof(DiscosModelBase)) ||
																	 (p.PropertyType.GetGenericArguments().FirstOrDefault()?.IsAssignableTo(typeof(DiscosModelBase)) ?? false));
		props.ToList().ForEach(p => AddInclude(p.Name));
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddPageSize(int numPages)
	{
		_numPages = numPages;
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddPageNum(int pageNum)
	{
		_pageNum = pageNum;
		return this;
	}

	public IDiscosQueryBuilder<TObject> Reset()
	{
		_filterTree = new();
		_includes   = new();
		_numPages   = null;
		_pageNum    = null;
		return this;
	}

	public string Build()
	{
		bool firstDone = false;

		void AddJoiningChar(StringBuilder builder)
		{
			if (firstDone)
			{
				builder.Append('&');
			}
			else
			{
				firstDone = true;
				builder.Append('?');
			}
		}

		StringBuilder builder = new();
		if (!_filterTree.IsEmpty)
		{
			AddJoiningChar(builder);
			AddFilterString(builder);
		}
		if (_includes.Count > 0)
		{
			AddJoiningChar(builder);
			AddIncludeString(builder);
		}
		if (_numPages is not null)
		{
			AddJoiningChar(builder);
			AddNumPagesString(builder);
		}
		if (_pageNum is not null)
		{
			AddJoiningChar(builder);
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
		builder.Append(string.Join(',', _includes.Distinct()));
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
