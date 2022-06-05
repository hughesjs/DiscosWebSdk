using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class OperationNode: FilterTreeNode
{
	public override OperationNodeData? Data => _data as OperationNodeData;

	public OperationNode(FilterTree tree, BinaryTreeNode<FilterTreeNodeData>? parent, FilterTreeNodeData? data) : base(tree, parent, data) { }

	public OperationNode(){}

}
