using DiscosWebSdk.Exceptions;
using DiscosWebSdk.Interfaces.Queries;
using DiscosWebSdk.Models.ResponseModels.Entities;

namespace DiscosWebSdk.Services.Queries;

/// <summary>
/// This class verifies that queries that look valid actually are.
/// There are a few syntactically valid queries that the ESA has forbidden.
/// </summary>
internal class QueryErrataVerificationService: IQueryVerificationService
{
	private static readonly string[] ForbiddenInEntitiesIncludes = {"launches", "objects"};
	public                  void         CheckQuery<T>(string queryString) => CheckQuery(typeof(T), queryString);

	public void CheckQuery(Type t, string queryString)
	{
		if (!t.IsAssignableTo(typeof(Entity)))
		{
			return;
		}
		string[] includes = queryString.Replace("?include=","").Split(',');
		if (includes.Any(i => ForbiddenInEntitiesIncludes.Contains(i)))
		{
			throw new EsaDosProtectionException(queryString);
		}
	}
}
