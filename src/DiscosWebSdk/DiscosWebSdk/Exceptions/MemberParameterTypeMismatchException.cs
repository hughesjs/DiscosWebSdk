namespace DiscosWebSdk.Exceptions;

public class MemberParameterTypeMismatchException : Exception
{

	public MemberParameterTypeMismatchException(Type memberFieldType, Type parameterFieldType, string message = "") :
		base($"Member type ({memberFieldType.Name}) does not match Parameter type ({parameterFieldType.Name})")
	{
		MemberFieldType    = memberFieldType;
		ParameterFieldType = parameterFieldType;
	}

	public Type MemberFieldType    { get; }
	public Type ParameterFieldType { get; }
}
