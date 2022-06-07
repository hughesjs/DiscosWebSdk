using DISCOSweb_Sdk.Exceptions.Queries.Filters.FilterTree;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class OperationNode: FilterTreeNode
{
	public override OperationNodeData Data => (OperationNodeData)base.Data! ?? throw new InvalidFilterTreeException("Operation Node must have data");

	public OperationNode(FilterTree tree, FilterTreeNode? parent, OperationNodeData? data) : base(tree, parent, data) { }

}
