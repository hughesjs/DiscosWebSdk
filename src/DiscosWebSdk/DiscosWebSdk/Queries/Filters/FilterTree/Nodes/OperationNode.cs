using DiscosWebSdk.Exceptions.Queries.Filters.FilterTree;
using DiscosWebSdk.Queries.Filters.FilterTree.Data;

namespace DiscosWebSdk.Queries.Filters.FilterTree.Nodes;

internal class OperationNode : FilterTreeNode
{

	public OperationNode(FilterTree tree, FilterTreeNode? parent, OperationNodeData? data) : base(tree, parent, data) { }
	public override OperationNodeData Data => (OperationNodeData)base.Data! ?? throw new InvalidFilterTreeException("Operation Node must have data");
}
