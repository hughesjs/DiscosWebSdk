using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class DefinitionNode<TObject, TParam>: FilterTreeNode where TObject: notnull
{
	public override DefinitionNodeData<TObject, TParam>? Data => _data as DefinitionNodeData<TObject, TParam>;
	public DefinitionNode(FilterTree tree, DefinitionNode<TObject,TParam>? parent, DefinitionNodeData? data) : base(tree, parent, data) { }
	
	public DefinitionNode() {}
}


internal class DefinitionNode : FilterTreeNode
{
	public override DefinitionNodeData? Data => base.Data as DefinitionNodeData;

	public override DefinitionNode SetLeftChild(BinaryTreeNode node)
	{
		throw new InvalidOperationException("Filter definitions cannot have child nodes.");
	}

	public override DefinitionNode SetRightChild(BinaryTreeNode node)
	{
		throw new InvalidOperationException("Filter definitions cannot have child nodes.");
	}
	
	public DefinitionNode(FilterTree tree, FilterTreeNode? parent, DefinitionNodeData? data) : base(tree, parent, data) { }
}
