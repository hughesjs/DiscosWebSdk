using DiscosWebSdk.DataStructures.BinaryTrees;
using DiscosWebSdk.Queries.Filters.FilterTree.Data;

namespace DiscosWebSdk.Queries.Filters.FilterTree.Nodes;

internal class DefinitionNode<TObject, TParam> : FilterTreeNode where TObject : notnull
{
	public DefinitionNode(FilterTree tree, DefinitionNode<TObject, TParam>? parent, DefinitionNodeData? data) : base(tree, parent, data) { }

	public DefinitionNode() { }
	public override DefinitionNodeData<TObject, TParam>? Data => _data as DefinitionNodeData<TObject, TParam>;
}


internal class DefinitionNode : FilterTreeNode
{

	public DefinitionNode(FilterTree tree, FilterTreeNode? parent, DefinitionNodeData? data) : base(tree, parent, data) { }
	public override DefinitionNodeData? Data => base.Data as DefinitionNodeData;

	public override DefinitionNode SetLeftChild(BinaryTreeNode node) => throw new InvalidOperationException("Filter definitions cannot have child nodes.");

	public override DefinitionNode SetRightChild(BinaryTreeNode node) => throw new InvalidOperationException("Filter definitions cannot have child nodes.");
}
