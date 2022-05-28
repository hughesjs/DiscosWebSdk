using System.Reflection;
using System.Text.Json.Serialization;

namespace DISCOSweb_Sdk.Misc;

internal static class AttributeUtilities
{
	private static string GetJsonPropertyName(MemberInfo field)
	{
		JsonPropertyNameAttribute? att = field.GetCustomAttribute(typeof(JsonPropertyNameAttribute)) as JsonPropertyNameAttribute;
		return att?.Name ?? field.Name;
	}

	public static string GetJsonPropertyName<T>(string fieldName)
	{
		FieldInfo? field = typeof(T).GetField(fieldName);
		return field is null ? fieldName : GetJsonPropertyName(field);
	}
}
