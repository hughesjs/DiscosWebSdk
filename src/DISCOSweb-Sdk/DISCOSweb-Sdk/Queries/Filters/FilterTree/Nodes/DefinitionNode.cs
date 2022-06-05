using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

internal class DefinitionNode: BinaryTreeNode<DefinitionNodeData>
{


	public DefinitionNode(BinaryTreeNode<DefinitionNodeData>? parent, DefinitionNodeData? data) : base(parent, data) { }
}
