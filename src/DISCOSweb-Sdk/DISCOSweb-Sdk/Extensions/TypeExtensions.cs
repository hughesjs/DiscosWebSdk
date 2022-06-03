using System.Reflection;

namespace DISCOSweb_Sdk.Extensions;

internal static class TypeExtensions
{
	internal static void EnsureFieldExists(this Type type, string fieldName)
	{
		if (type.GetField(fieldName) is null && type.GetProperty(fieldName) is null)
		{
			throw new MissingFieldException($"Field ({fieldName}) does not exist on type {type.Name}");
		}
	}

	internal static void EnsureFieldIsOfType(this Type objectType, string fieldName, Type fieldType)
	{
		objectType.EnsureFieldExists(fieldName);
		FieldInfo? fieldInfo = objectType.GetField(fieldName);
		if (fieldInfo is not null && fieldInfo.FieldType != fieldType)
		{
			throw new MissingFieldException($"Field ({fieldName}) is not of type ({fieldType})");
		}

		PropertyInfo? propInfo = objectType.GetProperty(fieldName);
		if (propInfo is not null && propInfo.PropertyType != fieldType)
		{
			throw new MissingFieldException($"Field ({fieldName}) is not of type ({fieldType})");
		}
	}
	
}
