namespace DiscosWebSdk.Exceptions;

public class NotDiscosTypeException: Exception
{
	private Type ErroneousType { get; }

	public NotDiscosTypeException(Type erroneousType) : base($"{erroneousType.Name} is not a DiscosType")
	{
		ErroneousType = erroneousType;
	}
}