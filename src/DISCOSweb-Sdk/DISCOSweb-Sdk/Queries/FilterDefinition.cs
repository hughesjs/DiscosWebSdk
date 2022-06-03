using System.Collections;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Extensions;
using DISCOSweb_Sdk.Misc;

namespace DISCOSweb_Sdk.Queries;

public abstract record FilterDefinition
{
	public abstract override string ToString();
}

public record FilterDefinition<TObject, TParam>: FilterDefinition where TObject: notnull
{
	public string FieldName { get; }
	public TParam? Value { get; }
	public DiscosFunction Function { get; }
	public FilterDefinition(string fieldName, TParam? value, DiscosFunction function)
	{
		ValidateParams(fieldName, function);
		FieldName = fieldName;
		Value = value;
		Function = function;
	}

	private void ValidateParams(string fieldName, DiscosFunction function)
	{
		typeof(TObject).EnsureFieldExists(fieldName);
		if (typeof(TParam).IsCollectionType() && DiscosFuncAcceptsArray(function))
		{
			Type elementType = typeof(TParam).GetElementType() ?? throw new("Could not determine collection element type");
			typeof(TObject).EnsureFieldIsOfType(fieldName, elementType);
		}
		else
		{
			typeof(TObject).EnsureFieldIsOfType(fieldName, typeof(TParam));
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
			return $"{Function.GetEnumMemberValue()}({GetFieldNameJsonProperty()},{(val ? "true": "false")})";
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
				elements = ((IEnumerable)Value!).Cast<string>();
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
