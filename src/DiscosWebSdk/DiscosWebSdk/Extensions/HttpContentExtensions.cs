using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using DiscosWebSdk.Models.ResponseModels;
using Hypermedia.Metadata;

namespace DiscosWebSdk.Extensions;

internal static class HttpContentExtensions
{
	private static readonly MethodInfo UnconstructedReadAsJsonApiAsync = typeof(Hypermedia.JsonApi.Client.HttpContentExtensions)
																		.GetDisambiguatedMethodInfo(nameof(ReadAsJsonApiAsync),
																									new []
																									{
																										typeof(HttpContent),
																										typeof(IContractResolver)
																									},
																									1)!;

	private static readonly MethodInfo UnconstructedReadAsJsonApiManyAsync = typeof(Hypermedia.JsonApi.Client.HttpContentExtensions)
																			.GetDisambiguatedMethodInfo(nameof(ReadAsJsonApiManyAsync),
																										new []
																										{
																											typeof(HttpContent),
																											typeof(IContractResolver)
																										},
																										1)!;
	private static MethodInfo GetReadMethodInfo(Type t) => UnconstructedReadAsJsonApiAsync.MakeGenericMethod(t);
	private static MethodInfo GetReadManyMethodInfo(Type t) => UnconstructedReadAsJsonApiManyAsync.MakeGenericMethod(t);
	
	public static async Task<DiscosModelBase?> ReadAsJsonApiAsync(this HttpContent content, Type t, IContractResolver resolver) => await GetReadMethodInfo(t).InvokeAsync(null, content, resolver) as DiscosModelBase;

	public static async Task<IReadOnlyList<DiscosModelBase?>?> ReadAsJsonApiManyAsync(this HttpContent content, Type t, IContractResolver resolver)
	{
		object? res = await GetReadManyMethodInfo(t).InvokeAsync(null, content, resolver);
		if (res is null) return new ReadOnlyCollection<DiscosModelBase?>(new List<DiscosModelBase?>());
		IReadOnlyList <DiscosModelBase?> resList = ((IEnumerable)res).Cast<DiscosModelBase?>().ToArray();
		return resList;
	}
}
