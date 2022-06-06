namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

internal abstract class FilterTreeNodeData
{
	protected FilterTreeNodeData(object? value)
	{
		Value = value;
	}
	public virtual object? Value { get; }
}
