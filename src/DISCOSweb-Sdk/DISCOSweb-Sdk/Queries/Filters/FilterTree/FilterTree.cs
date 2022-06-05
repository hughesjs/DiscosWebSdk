using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree;

internal class FilterTree: BinaryTree<FilterTreeNodeData>
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
