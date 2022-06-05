using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class DefinitionNode<TObject, TParam>: FilterTreeNode
{

	public override DefinitionNodeData<TObject, TParam>? Data => _data as DefinitionNodeData<TObject, TParam>;
	public DefinitionNode(FilterTree tree, BinaryTreeNode<FilterTreeNodeData>? parent, FilterTreeNodeData? data) : base(tree, parent, data) { }
	
	public DefinitionNode() {}
}
