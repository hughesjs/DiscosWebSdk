using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal abstract class FilterTreeNode: BinaryTreeNode<FilterTreeNodeData>
{
	public FilterTreeNode(FilterTree tree, FilterTreeNode? parent, FilterTreeNodeData? data) : base(tree, parent, data) { }

	public override FilterTreeNode? LeftChild => base.LeftChild as FilterTreeNode;
	public override FilterTreeNode? RightChild => base.RightChild as FilterTreeNode;
	public override FilterTreeNode? Parent => base.Parent as FilterTreeNode;
	
	protected FilterTreeNode() { }
}

