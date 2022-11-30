namespace DiscosWebSdk.Interfaces.Queries;

public interface IQueryVerificationService
{
	public void CheckQuery<T>(string queryString);
	public void CheckQuery(Type      queryString, string s);
}
