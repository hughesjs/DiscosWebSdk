namespace DISCOSweb_Sdk.Queries.Filters.Tree;

public abstract class FilterNode
{
	public FilterNode? Parent { get; set; }
	public List<FilterNode> Children { get; }
	public FilterNode()
	{
		Children = new();
	}
	public virtual FilterNode AddChild(FilterNode node)
	{
		node.Parent = this;
		Children.Add(node);
		return node;
	}

	public int CountNodes()
	{
		int count = Children.Count;
		foreach (FilterNode child in Children)
		{
			if (child.Children.Count > 0)
			{
				count += child.CountNodes();
			}
		}
		return count;
	}
}
