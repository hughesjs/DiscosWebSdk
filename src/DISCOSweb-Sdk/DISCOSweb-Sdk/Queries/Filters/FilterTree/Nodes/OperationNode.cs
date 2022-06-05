using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class OperationNode: BinaryTreeNode<OperationNodeData>
{

	public OperationNode(BinaryTreeNode<OperationNodeData>? parent, OperationNodeData? data) : base(parent, data) { }
}
