using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal abstract class FilterTreeNode: BinaryTreeNode<FilterTreeNodeData>
{
	public FilterTreeNode(BinaryTreeNode<FilterTreeNodeData>? parent, FilterTreeNodeData? data) : base(parent, data) { }
}

