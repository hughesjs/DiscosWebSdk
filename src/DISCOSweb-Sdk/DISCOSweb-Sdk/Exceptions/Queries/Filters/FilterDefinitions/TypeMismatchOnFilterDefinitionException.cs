namespace DISCOSweb_Sdk.Exceptions.Queries.Filters.FilterDefinitions;

public class TypeMismatchOnFilterDefinitionException: MemberParameterTypeMismatchException
{

	public TypeMismatchOnFilterDefinitionException(Type memberFieldType, Type parameterFieldType, string message = "") : base(memberFieldType, parameterFieldType, message) { }
}
