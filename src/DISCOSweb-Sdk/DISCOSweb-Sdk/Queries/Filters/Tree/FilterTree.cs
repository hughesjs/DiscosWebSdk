namespace DISCOSweb_Sdk.Queries.Filters.Tree;

public class FilterTree
{
	public FilterTree()
	{
		RootNode = new RootNode();
	}
	public FilterNode RootNode { get; }

	public int CountNodes()
	{
		return RootNode.CountNodes();
	}
}
