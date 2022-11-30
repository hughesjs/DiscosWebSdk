namespace DiscosWebSdk.Exceptions;

public class EsaDosProtectionException: Exception
{
	public string QueryString { get; }

	public EsaDosProtectionException(string queryString) : base("Can't include Launches or Objects on an Entities query because the ESA can't cope with the data load")
	{
		QueryString = queryString;
	}
}
