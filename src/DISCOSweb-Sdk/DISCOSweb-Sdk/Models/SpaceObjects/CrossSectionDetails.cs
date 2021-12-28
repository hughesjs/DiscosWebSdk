namespace DISCOSweb_Sdk.Models.SpaceObjects;

/// <summary>
/// Details of the object's cross sectional area in m2
/// </summary>
public record CrossSectionDetails
{
	public double Maximum { get; init; }
	public double Minimum { get; init; }
	public double Average { get; init; }
}
