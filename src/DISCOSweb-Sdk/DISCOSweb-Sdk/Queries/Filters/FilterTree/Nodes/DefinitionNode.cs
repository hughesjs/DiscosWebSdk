using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class DefinitionNode<TObject, TParam>: FilterTreeNode
{
	
	public override DefinitionNodeData<TObject, TParam>? Data { get; }
	public DefinitionNode(BinaryTreeNode<FilterTreeNodeData>? parent, FilterTreeNodeData? data) : base(parent, data) { }
}
