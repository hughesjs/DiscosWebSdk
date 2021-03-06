using System.Collections;
using DiscosWebSdk.Enums;
using DiscosWebSdk.Exceptions;
using DiscosWebSdk.Exceptions.Queries.Filters.FilterDefinitions;
using DiscosWebSdk.Extensions;
using DiscosWebSdk.Misc;

namespace DiscosWebSdk.Queries.Filters;

public abstract record FilterDefinition
{
	public abstract override string ToString();
}


public record FilterDefinition<TObject, TParam> : FilterDefinition where TObject : notnull
{

	public FilterDefinition(string fieldName, TParam? value, DiscosFunction function)
	{
		ValidateParams(fieldName, function);
		FieldName = fieldName;
		Value     = value;
		Function  = function;
	}

	public string         FieldName { get; }
	public TParam?        Value     { get; }
	public DiscosFunction Function  { get; }

	private void ValidateParams(string fieldName, DiscosFunction function)
	{
		try
		{
			typeof(TObject).EnsureFieldExists(fieldName);
			if (typeof(TParam).IsCollectionType())
			{
				if (!DiscosFuncAcceptsArray(function))
				{
					throw new DiscosFunctionDoesntSupportArraysException(function);
				}
				Type elementType = typeof(TParam).GetElementType() ?? throw new("Could not determine collection element type");
				typeof(TObject).EnsureFieldIsOfType(fieldName, elementType);
			}
			else
			{
				typeof(TObject).EnsureFieldIsOfType(fieldName, typeof(TParam));
			}
		}
		catch (MissingMemberException)
		{
			throw new InvalidPropertyOnFilterDefinitionException(typeof(TObject).Name, fieldName);
		}
		catch (MemberParameterTypeMismatchException)
		{
			throw new TypeMismatchOnFilterDefinitionException(typeof(TObject), typeof(TParam));
		}

	}

	private bool DiscosFuncAcceptsArray(DiscosFunction function)
	{
		switch (function)
		{
			case DiscosFunction.Includes:
			case DiscosFunction.DoesNotInclude:
			case DiscosFunction.Excludes:
			case DiscosFunction.Contains:
			case DiscosFunction.InsensitiveContains:
			case DiscosFunction.InsensitiveExcludes:
			{
				return true;
			}
			default:
			{
				return false;
			}
		}
	}

	public override string ToString()
	{
		if (Value is null)
		{
			return $"{Function.GetEnumMemberValue()}({GetFieldNameJsonProperty()},null)";
		}

		if (Value is bool val)
		{
			return $"{Function.GetEnumMemberValue()}({GetFieldNameJsonProperty()},{(val ? "true" : "false")})";
		}

		if (typeof(TParam).IsCollectionType())
		{
			IEnumerable<string> elements;
			// We can suppress the null checks below as we know these casts are safe
			if (typeof(TParam).GetElementType() == typeof(string))
			{
				elements = ((IEnumerable<string>)Value!).Select(e => $"'{e}'");
			}
			else
			{
				elements = ((IEnumerable)Value).Cast<object?>().Select(x => x.ToString()).ToList();
			}
			return $"{Function.GetEnumMemberValue()}({GetFieldNameJsonProperty()},({string.Join(',', elements.ToArray())}))";
		}

		if (typeof(TParam).IsNumericType())
		{
			return $"{Function.GetEnumMemberValue()}({GetFieldNameJsonProperty()},{Value})";
		}

		return $"{Function.GetEnumMemberValue()}({GetFieldNameJsonProperty()},'{Value}')";
	}

	private string GetFieldNameJsonProperty() => AttributeUtilities.GetJsonPropertyName<TObject>(FieldName);
}
