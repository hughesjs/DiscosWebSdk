using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree;

internal class FilterTree: BinaryTree<FilterTreeNode>
{
	

	// private void AddOperationNode(NodeOperation op)
	// {
	// 	if (_currentNode is RootNode)
	// 	{
	// 		throw new InvalidFilterTreeException("Operation can't be performed on root node");
	// 	}
	// 	FilterNode branchPoint = _currentNode.Parent;
	// 	FilterNode prevCurrent = _currentNode;
	// 	prevCurrent.BecomeOrphan();
	// 	FilterNode opNode = branchPoint.AddChild(new OperationNode(op));
	// 	opNode.AddChild(prevCurrent);
	// 	_currentNode = opNode;
	// }
}
