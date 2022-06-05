using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal abstract class FilterTreeNode: BinaryTreeNode<FilterTreeNodeData>
{
	public FilterTreeNode(FilterTree tree, BinaryTreeNode<FilterTreeNodeData>? parent, FilterTreeNodeData? data) : base(tree, parent, data) { }

	protected FilterTreeNode() { }
}

