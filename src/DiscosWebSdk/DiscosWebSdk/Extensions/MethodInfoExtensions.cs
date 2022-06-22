using System.Reflection;

namespace DiscosWebSdk.Extensions;

internal static class MethodInfoExtensions
{
	internal static async Task<object?> InvokeAsync(this MethodInfo method, object? obj, params object[] parameters)
	{
		Task? invokedRead = (Task?)method.Invoke(obj, parameters);
		if (invokedRead is null) return null;
		PropertyInfo? resProperty = invokedRead.GetType().GetProperty("Result");
		await invokedRead;
		return resProperty?.GetValue(invokedRead);
	}
}
