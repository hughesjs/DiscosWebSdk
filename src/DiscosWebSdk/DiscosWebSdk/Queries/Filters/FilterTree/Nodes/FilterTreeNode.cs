using DiscosWebSdk.DataStructures.BinaryTrees;
using DiscosWebSdk.Queries.Filters.FilterTree.Data;

namespace DiscosWebSdk.Queries.Filters.FilterTree.Nodes;

internal abstract class FilterTreeNode : BinaryTreeNode<FilterTreeNodeData>
{
	public FilterTreeNode(FilterTree tree, FilterTreeNode? parent, FilterTreeNodeData? data) : base(tree, parent, data) { }

	protected FilterTreeNode() { }

	public override FilterTreeNode? LeftChild  => base.LeftChild as FilterTreeNode;
	public override FilterTreeNode? RightChild => base.RightChild as FilterTreeNode;
	public override FilterTreeNode? Parent     => base.Parent as FilterTreeNode;
}
