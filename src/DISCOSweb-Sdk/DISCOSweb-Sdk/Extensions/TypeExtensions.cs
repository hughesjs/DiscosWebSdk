namespace DISCOSweb_Sdk.Extensions;

internal static class TypeExtensions
{
	internal static void EnsureFieldExists(this Type type, string fieldName)
	{
		if (type.GetField(fieldName) is null && type.GetProperty(fieldName) is not null)
		{
			throw new MissingFieldException($"Field ({fieldName}) does not exist on type {type.Name}");
		}
	}
	
}
