using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DiscosWebSdk.Misc;

internal static class AttributeUtilities
{


	public static string GetJsonPropertyName<T>(string fieldName) => GetJsonPropertyName(typeof(T), fieldName);
	
	public static string GetJsonPropertyName(Type t, string fieldName)
	{
		PropertyInfo? field = t.GetProperty(fieldName);
		return field is null ? fieldName : GetJsonPropertyName(field);
	}
	
	public static string GetJsonPropertyName(MemberInfo field)
	{
		JsonPropertyNameAttribute? att = field.GetCustomAttribute(typeof(JsonPropertyNameAttribute)) as JsonPropertyNameAttribute;
		return att?.Name ?? field.Name;
	}

	public static string? GetEnumMemberValue<T>(this T value)
		where T : Enum
	{
		return typeof(T)
			  .GetTypeInfo()
			  .DeclaredMembers
			  .SingleOrDefault(x => x.Name == value.ToString())
			 ?.GetCustomAttribute<EnumMemberAttribute>(false)
			 ?.Value;
	}
}
