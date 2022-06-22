using System.Collections;
using System.Reflection;
using DiscosWebSdk.Exceptions;

namespace DiscosWebSdk.Extensions;

internal static class InternalTypeExtensions
{
	internal static void EnsureFieldExists(this Type type, string fieldName)
	{
		if (type.GetField(fieldName) is null && type.GetProperty(fieldName) is null)
		{
			throw new MissingMemberException($"Field ({fieldName}) does not exist on type {type.Name}");
		}
	}

	internal static void EnsureFieldIsOfType(this Type objectType, string fieldName, Type fieldType, bool checkNullability = false)
	{
		FieldInfo? fieldInfo       = objectType.GetField(fieldName);
		Type?      objectFieldType = fieldInfo?.FieldType;

		PropertyInfo? propInfo       = objectType.GetProperty(fieldName);
		Type?         objectPropType = propInfo?.PropertyType;

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
			throw new MemberParameterTypeMismatchException(objectFieldType, fieldType);
		}


		if (objectPropType is not null && objectPropType != fieldType)
		{
			throw new MemberParameterTypeMismatchException(objectPropType, fieldType);
		}
	}

	internal static bool IsNumericType(this Type t)
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

	internal static bool IsCollectionType(this Type t) => t != typeof(string) && t.GetInterface(nameof(IEnumerable)) is not null;

	internal static MethodInfo? GetDisambiguatedMethodInfo(this Type t, string name, Type[]? parameterTypes = null, int numGenericArgs = 0)
	{
		MethodInfo[] methods = t.GetMethods();
		if (methods.Length == 0) return null;
		if (methods.Length == 1) return methods[0];

		foreach (MethodInfo method in methods)
		{
			if (method.Name != name) continue;
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != parameterTypes?.Length) continue;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].ParameterType != parameterTypes[i]) goto innerContinue;
			}
			if (numGenericArgs == 0 && method.IsGenericMethod) continue;
			if (numGenericArgs == method.GetGenericArguments().Length) return method;
			innerContinue:;
		}

		return null;
	}
}


