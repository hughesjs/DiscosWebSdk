using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Exceptions;

namespace DISCOSweb_Sdk.Queries.Filters.Tree;

public class OperationNode: FilterNode
{
	public OperationNode(NodeOperation operation)
	{
		Operation = operation;
	}
	public NodeOperation Operation { get; }

	public override FilterNode AddChild(FilterNode node)
	{
		if (Children.Count > 1)
		{
			throw new InvalidFilterTreeException("Operation Nodes Can Only Have 2 Children");
		}
		return base.AddChild(node);
	}
}
