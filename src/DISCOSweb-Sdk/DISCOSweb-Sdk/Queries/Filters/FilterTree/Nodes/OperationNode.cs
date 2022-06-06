using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class OperationNode: FilterTreeNode
{
	public override OperationNodeData? Data => base.Data as OperationNodeData;

	public OperationNode(FilterTree tree, FilterTreeNode? parent, OperationNodeData? data) : base(tree, parent, data) { }

	public OperationNode(){}

}
