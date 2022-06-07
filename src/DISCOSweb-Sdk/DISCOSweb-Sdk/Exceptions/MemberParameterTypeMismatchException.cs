namespace DISCOSweb_Sdk.Exceptions;

public class MemberParameterTypeMismatchException : Exception
{
	public Type MemberFieldType { get; }
	public Type ParameterFieldType { get; }

	public MemberParameterTypeMismatchException(Type memberFieldType, Type parameterFieldType, string message = "") :
		base($"Member type ({memberFieldType.Name}) does not match Parameter type ({parameterFieldType.Name})")
	{
		MemberFieldType = memberFieldType;
		ParameterFieldType = parameterFieldType;
	}

}
