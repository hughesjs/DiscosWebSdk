using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree;

internal class FilterTree: BinaryTree<FilterTreeNode>
{
	public void AddOperationNode(FilterOperation operation)
	{
		throw new NotImplementedException();
	}

	public void AddDefinitionNode(FilterDefinition definition)
	{
		throw new NotImplementedException();
	}

	public FilterTree(FilterTreeNode rootNode) : base(rootNode) { }
	public FilterTree() {}
}
