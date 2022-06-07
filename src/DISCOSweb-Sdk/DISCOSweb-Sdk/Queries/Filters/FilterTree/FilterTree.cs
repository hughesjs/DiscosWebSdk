using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Exceptions.Queries.Filters.FilterTree;
using DISCOSweb_Sdk.Misc;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree;

internal class FilterTree : BinaryTree<FilterTreeNode>
{
	public OperationNode AddOperationNode(FilterOperation operation)
	{
		OperationNode newNode = new(this, null, new(operation));
		switch (Head) // Deal with current head
		{
			case null:
			{
				throw new InvalidFilterTreeException("Cannot add Operation Node before the first definition");
			}
			case OperationNode:
			{
				throw new InvalidFilterTreeException("Need to add Operation Node after Filter Node");
			}
		}

		if (Head == Root || (Head.Parent!.LeftChild is DefinitionNode && Head.Parent.RightChild is DefinitionNode))
		{
			return (OperationNode)SetRoot(newNode);
		}

		return (OperationNode)InsertNodeAtHead(newNode);
	}

	public DefinitionNode AddDefinitionNode(FilterDefinition definition)
	{
		DefinitionNode newNode = new(this, null, new(definition));
		if (Root is null)
		{
			SetRoot(newNode);
			return newNode;
		}
		if (Head is DefinitionNode)
		{
			throw new InvalidFilterTreeException("Definition node cannot be added to another definition node");
		}
		if (Head is null)
		{
			throw new InvalidOperationException("Head is detached from tree");
		}

		// Head must be Operation Node
		return (DefinitionNode)Head.SetRightChild(newNode);
	}

	public bool IsCompleteTree()
	{
		if (Root is null)
		{
			return false;
		}
		return IsCompleteSubTree(Root);
	}

	private bool IsCompleteSubTree(FilterTreeNode startNode)
	{
		if (startNode is DefinitionNode)
		{
			return true;
		}

		if (startNode.LeftChild is null || startNode.RightChild is null)
		{
			return false;
		}

		return IsCompleteSubTree(startNode.LeftChild) && IsCompleteSubTree(startNode.RightChild);
	}

	public override string ToString()
	{
		if (IsEmpty)
		{
			return "Empty Tree";
		}
		return GetSubTreeString(Root!);
	}

	private string GetSubTreeString(FilterTreeNode? startNode)
	{
		switch (startNode)
		{
			case DefinitionNode node:
				return node.ToString() ?? string.Empty;
			case OperationNode node:
				return $"{node.Data.Value.GetEnumMemberValue()}({GetSubTreeString(node.LeftChild)},{GetSubTreeString(node.RightChild)})";
			case null:
				return string.Empty;
			default:
				throw new InvalidFilterTreeException("Unknown node type in tree");
		}
	}

	public FilterTree(FilterTreeNode rootNode) : base(rootNode) { }
	public FilterTree() { }
}
