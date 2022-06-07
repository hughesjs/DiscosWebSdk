namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

internal abstract class FilterTreeNodeData
{
	
}


internal abstract class FilterTreeNodeData<T>: FilterTreeNodeData
{
	protected FilterTreeNodeData(T value) => Value = value;
	public T Value { get; }

	public override string ToString() => Value?.ToString() ?? base.ToString()!;
}
