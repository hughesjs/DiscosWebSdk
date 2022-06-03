using DISCOSweb_Sdk.Exceptions;

namespace DISCOSweb_Sdk.Queries.Filters.Tree;

public class RootNode:FilterNode
{
	public RootNode()
	{
		Parent = this;
	}

	public override FilterNode AddChild(FilterNode node)
	{
		if (Children.Count > 0)
		{
			throw new InvalidFilterTreeException("Root node may only have a single child");
		}
		Children.Add(node);
		return node;
	}
}
