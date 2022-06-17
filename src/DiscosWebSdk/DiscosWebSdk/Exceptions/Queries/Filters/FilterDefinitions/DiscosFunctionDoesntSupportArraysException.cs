using DiscosWebSdk.Enums;

namespace DiscosWebSdk.Exceptions.Queries.Filters.FilterDefinitions;

public class DiscosFunctionDoesntSupportArraysException : Exception
{

	public DiscosFunctionDoesntSupportArraysException(DiscosFunction function) : base($"DISCOS Function ({function}) doesn't support arrays. Check your filter.") => Function = function;

	public DiscosFunction Function { get; }
}
