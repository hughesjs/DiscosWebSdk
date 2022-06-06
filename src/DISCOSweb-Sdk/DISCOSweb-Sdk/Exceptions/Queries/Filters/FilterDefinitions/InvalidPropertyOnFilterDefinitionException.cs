namespace DISCOSweb_Sdk.Exceptions.Queries.Filters.FilterDefinitions;

public class InvalidPropertyOnFilterDefinitionException: MissingMemberException
{
	public InvalidPropertyOnFilterDefinitionException(string className, string memberName, Exception? inner = null) : base($"{className} does not have {memberName} member.\nCheck your filter definition.", inner){}
}
