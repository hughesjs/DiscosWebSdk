using System.Reflection;
using DiscosWebSdk.Mapping.JsonApi;

namespace DiscosWebSdk.Extensions;

internal static class HttpContentExtensions
{
	private static readonly MethodInfo UnconstructedReadAsJsonApiAsync = typeof(HttpContentExtensions)
																		.GetMethods()
																		.Single(m =>
																					m.Name == nameof(ReadAsJsonApiAsync) && m.GetParameters().Length == 2 && m.GetGenericArguments().Length == 1);

	private static MethodInfo GetReadMethodInfo(Type t) => UnconstructedReadAsJsonApiAsync.MakeGenericMethod(t);

	public static async Task<object?> ReadAsJsonApiAsync(this HttpContent content, Type t) => await GetReadMethodInfo(t).InvokeAsync(null, content, DiscosObjectResolver.CreateResolver());
}
