using DiscosWebSdk.Models.ResponseModels;
using DiscosWebSdk.Models.ResponseModels.Orbits;

namespace DiscosWebSdk.Extensions;

public static class TypeExtensions
{
	public static bool IsDiscosModel(this Type t, bool includeBase = false)
	{
		if (t == typeof(DiscosModelBase) || t == typeof(OrbitDetails)) return includeBase;

		if (t.IsAssignableTo(typeof(DiscosModelBase))) return true;

		if (t.HasElementType)
		{
			Type elementType = t.GetElementType()!;
			if (elementType == typeof(DiscosModelBase)) return includeBase;
			if (elementType.IsAssignableTo(typeof(DiscosModelBase))) return true;
		}

		if (t.IsCollectionType() && t.IsGenericType)
		{
			Type genericArg = t.GetGenericArguments().Single();
			if (genericArg == typeof(DiscosModelBase)) return includeBase;
			if (genericArg.IsAssignableTo(typeof(DiscosModelBase))) return true;
		}

		return false;
	}
}
