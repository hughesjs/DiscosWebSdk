using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class OperationNode: FilterTreeNode
{
	public override OperationNodeData? Data { get; }

	public OperationNode(BinaryTreeNode<FilterTreeNodeData>? parent, FilterTreeNodeData? data) : base(parent, data) { }
}
