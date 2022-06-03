using System.Collections;
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

	internal static void EnsureFieldIsOfType(this Type objectType, string fieldName, Type fieldType, bool checkNullability = false)
	{
		
		objectType.EnsureFieldExists(fieldName);
		FieldInfo? fieldInfo = objectType.GetField(fieldName);
		Type? objectFieldType = fieldInfo?.FieldType;
		
		PropertyInfo? propInfo = objectType.GetProperty(fieldName);
		Type? objectPropType = propInfo?.PropertyType;
		
		if (!checkNullability)
		{
			fieldType = Nullable.GetUnderlyingType(fieldType) ?? fieldType;
			if (objectFieldType is not null)
			{
				objectFieldType = Nullable.GetUnderlyingType(objectFieldType) ?? objectFieldType;
			}
			if (objectPropType is not null)
			{
				objectPropType = Nullable.GetUnderlyingType(objectPropType) ?? objectPropType;
			}
		}
		
		if (objectFieldType is not null && objectFieldType != fieldType)
		{
			throw new MissingFieldException($"Field ({fieldName}) is not of type ({fieldType})");
		}

		
		if (objectPropType is not null && objectPropType != fieldType)
		{
			throw new MissingFieldException($"Field ({fieldName}) is not of type ({fieldType})");
		}
	}
	
	public static bool IsNumericType(this Type t)
	{
		t = Nullable.GetUnderlyingType(t) ?? t; // If nullable, get underlying type
		switch (Type.GetTypeCode(t))
		{
			case TypeCode.Byte:
			case TypeCode.SByte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Decimal:
			case TypeCode.Double:
			case TypeCode.Single:
				return true;
			default:
				return false;
		}
	}

	public static bool IsCollectionType(this Type t) => t != typeof(string) && t.GetInterface(nameof(IEnumerable)) is not null;
}
