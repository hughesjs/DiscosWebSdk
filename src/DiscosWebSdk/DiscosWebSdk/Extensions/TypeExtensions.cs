using DiscosWebSdk.Models.ResponseModels;

namespace DiscosWebSdk.Extensions;

public static class TypeExtensions
{
	public static bool IsDiscosModel(this Type t) =>
		t.IsAssignableTo(typeof(DiscosModelBase))                                           ||
		t.HasElementType     && t.GetElementType()!.IsAssignableTo(typeof(DiscosModelBase)) ||
		t.IsCollectionType() && t.IsGenericType && t.GetGenericArguments().Single().IsAssignableTo(typeof(DiscosModelBase));
}
