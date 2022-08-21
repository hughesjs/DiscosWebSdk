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

internal class DiscosQueryBuilder : IDiscosQueryBuilder
{
	private FilterTree   _filterTree;
	private List<string> _includes;
	private int?         _numPages;
	private int?         _pageNum;

	public DiscosQueryBuilder()
	{
		_filterTree = new();
		_includes   = new();
		_numPages = null;
		_pageNum = null;
	}

	private void AddPageNumberString(StringBuilder builder)
	{
		builder.Append($"page[number]={_pageNum}");
	}

	private void AddNumPagesString(StringBuilder builder)
	{
		builder.Append($"page[size]={_numPages}");
	}
	
	
	public virtual IDiscosQueryBuilder AddPageSize(int numPages)
	{
		_numPages = numPages;
		return this;
	}
	
	public virtual IDiscosQueryBuilder AddPageNum(int pageNum)
	{
		_pageNum = pageNum;
		return this;
	}

	public virtual IDiscosQueryBuilder Reset()
	{
		_filterTree = new();
		_includes   = new();
		_numPages   = null;
		_pageNum    = null;
		return this;
	}
	
	public virtual IDiscosQueryBuilder AddAllIncludes(Type t)
	{
		IEnumerable<PropertyInfo> props = t.GetProperties()
			.Where(p => p.PropertyType.IsAssignableTo(typeof(DiscosModelBase)) ||
			            (p.PropertyType.GetGenericArguments().FirstOrDefault()?.IsAssignableTo(typeof(DiscosModelBase)) ?? false));
		props.ToList().ForEach(p => AddInclude(t, p.Name));
		return this;
	}
	
	public virtual IDiscosQueryBuilder AddInclude(Type t, string fieldName)
	{
		t.EnsureFieldExists(fieldName);
		_includes.Add(AttributeUtilities.GetJsonPropertyName(t, fieldName));
		return this;
	}
	
	public virtual IDiscosQueryBuilder AddFilter(Type t, FilterDefinition filterDefinition)
	{
		if (filterDefinition.GetType().GetGenericArguments().First() != t)
		{
			throw new InvalidFilterTreeException("Filter object type must match query builder type");
		}
		_filterTree.AddDefinitionNode(filterDefinition);
		return this;
	}

	public virtual IDiscosQueryBuilder And()
	{
		_filterTree.AddOperationNode(FilterOperation.And);
		return this;
	}


	public virtual IDiscosQueryBuilder Or()
	{
		_filterTree.AddOperationNode(FilterOperation.Or);
		return this;
	}

	public virtual string Build()
	{
		bool firstDone = false;
		
		StringBuilder builder = new();
		if (!_filterTree.IsEmpty)
		{
			AddJoiningChar(builder, ref firstDone);
			AddFilterString(builder);
		}
		if (_includes.Count > 0)
		{
			AddJoiningChar(builder, ref firstDone);
			AddIncludeString(builder);
		}
		if (_numPages is not null)
		{
			AddJoiningChar(builder, ref firstDone);
			AddNumPagesString(builder);
		}
		if (_pageNum is not null)
		{
			AddJoiningChar(builder, ref firstDone);
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

	private void AddJoiningChar(StringBuilder builder, ref bool firstDone)
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
}

internal class DiscosQueryBuilder<TObject> : DiscosQueryBuilder, IDiscosQueryBuilder<TObject> where TObject : notnull
{
	public IDiscosQueryBuilder<TObject> AddInclude(string fieldName)
	{
		base.AddInclude(typeof(TObject), fieldName);
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddAllIncludes()
	{
		base.AddAllIncludes(typeof(TObject));
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddFilter(FilterDefinition filterDefinition)
	{
		base.AddFilter(typeof(TObject), filterDefinition);
		return this;
	}

	public override IDiscosQueryBuilder<TObject> And()
	{
		base.And();
		return this;
	}

	public override IDiscosQueryBuilder<TObject> Or()
	{
		base.Or();
		return this;
	}

	public override IDiscosQueryBuilder<TObject> AddPageSize(int numPages)
	{
		base.AddPageSize(numPages);
		return this;
	}

	public override IDiscosQueryBuilder<TObject> AddPageNum(int pageNum)
	{
		base.AddPageNum(pageNum);
		return this;
	}

	public sealed override IDiscosQueryBuilder<TObject> Reset()
	{
		base.Reset();
		return this;
	}
}
