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

internal class DiscosQueryBuilder<TObject> : DiscosQueryBuilder, IDiscosQueryBuilder<TObject> where TObject : notnull
{
	public IDiscosQueryBuilder<TObject> AddFilter(FilterDefinition filterDefinition)
	{
		if (filterDefinition.GetType().GetGenericArguments().First() != typeof(TObject))
		{
			throw new InvalidFilterTreeException("Filter object type must match query builder type");
		}
		FilterTree.AddDefinitionNode(filterDefinition);
		return this;
	}

	public IDiscosQueryBuilder<TObject> And()
	{
		FilterTree.AddOperationNode(FilterOperation.And);
		return this;
	}


	public IDiscosQueryBuilder<TObject> Or()
	{
		FilterTree.AddOperationNode(FilterOperation.Or);
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddInclude(string fieldName)
	{
		typeof(TObject).EnsureFieldExists(fieldName);
		Includes.Add(AttributeUtilities.GetJsonPropertyName<TObject>(fieldName));
		return this;
	}

	public IDiscosQueryBuilder<TObject> AddAllIncludes()
	{
		base.AddAllIncludes(typeof(TObject));
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
	
	

	public override string Build()
	{
		bool firstDone = false;
		
		StringBuilder builder = new();
		if (!FilterTree.IsEmpty)
		{
			AddJoiningChar(builder, ref firstDone);
			AddFilterString(builder);
		}
		if (Includes.Count > 0)
		{
			AddJoiningChar(builder, ref firstDone);
			AddIncludeString(builder);
		}
		if (NumPages is not null)
		{
			AddJoiningChar(builder, ref firstDone);
			AddNumPagesString(builder);
		}
		if (PageNum is not null)
		{
			AddJoiningChar(builder, ref firstDone);
			AddPageNumberString(builder);
		}
		return builder.ToString();
	}

	private void AddFilterString(StringBuilder builder)
	{
		if (!FilterTree.IsCompleteTree())
		{
			throw new InvalidFilterTreeException("Filter tree is not complete");
		}
		builder.Append("filter=");
		builder.Append(FilterTree);
	}

	private void AddIncludeString(StringBuilder builder)
	{
		builder.Append("include=");
		builder.Append(string.Join(',', Includes.Distinct()));
	}

	private void AddPageNumberString(StringBuilder builder)
	{
		builder.Append($"page[number]={PageNum}");
	}

	private void AddNumPagesString(StringBuilder builder)
	{
		builder.Append($"page[size]={NumPages}");
	}
}


internal class DiscosQueryBuilder : IDiscosQueryBuilder
{
	protected FilterTree   FilterTree;
	protected List<string> Includes;
	protected int?         NumPages;
	protected int?         PageNum;

	public DiscosQueryBuilder()
	{
		FilterTree = new();
		Includes   = new();
		NumPages = null;
		PageNum = null;
	}

	private void AddPageNumberString(StringBuilder builder)
	{
		builder.Append($"page[number]={PageNum}");
	}

	private void AddNumPagesString(StringBuilder builder)
	{
		builder.Append($"page[size]={NumPages}");
	}
	
	
	public virtual IDiscosQueryBuilder AddPageSize(int numPages)
	{
		NumPages = numPages;
		return this;
	}
	
	public virtual IDiscosQueryBuilder AddPageNum(int pageNum)
	{
		PageNum = pageNum;
		return this;
	}

	public virtual IDiscosQueryBuilder Reset()
	{
		FilterTree = new();
		Includes   = new();
		NumPages   = null;
		PageNum    = null;
		return this;
	}
	
	public IDiscosQueryBuilder AddAllIncludes(Type t)
	{
		IEnumerable<PropertyInfo> props = t.GetProperties()
			.Where(p => p.PropertyType.IsAssignableTo(typeof(DiscosModelBase)) ||
			            (p.PropertyType.GetGenericArguments().FirstOrDefault()?.IsAssignableTo(typeof(DiscosModelBase)) ?? false));
		props.ToList().ForEach(p => AddInclude(t, p.Name));
		return this;
	}
	
	public IDiscosQueryBuilder AddInclude(Type t, string fieldName)
	{
		t.EnsureFieldExists(fieldName);
		Includes.Add(AttributeUtilities.GetJsonPropertyName(t, fieldName));
		return this;
	}

	public virtual string Build()
	{
		bool firstDone = false;

		StringBuilder builder = new();
		
		if (NumPages is not null)
		{
			AddJoiningChar(builder, ref firstDone);
			AddNumPagesString(builder);
		}
		
		if (PageNum is not null)
		{
			AddJoiningChar(builder, ref firstDone);
			AddPageNumberString(builder);
		}

		return builder.ToString();
	}
	
	protected void AddJoiningChar(StringBuilder builder, ref bool firstDone)
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
