using DISCOSweb_Sdk.Exceptions.DataStructures.BinaryTree;

namespace DISCOSweb_Sdk.DataStructures.BinaryTrees;

internal class BinaryTree<TNodeType> : BinaryTree where TNodeType : BinaryTreeNode
{


	public BinaryTree() { }
	public BinaryTree(TNodeType rootNode) : base(rootNode) { }
	public override TNodeType? Root => _root as TNodeType;
	public override TNodeType? Head => _head as TNodeType;

	public override TNodeType SetRoot(BinaryTreeNode newRootNode)
	{
		if (newRootNode is not TNodeType node)
		{
			throw new InvalidNodeTypeException("Root must be of same type as tree.");
		}
		base.SetRoot(node);
		return node;
	}
}


internal class BinaryTree
{
	protected BinaryTreeNode? _head;
	protected BinaryTreeNode? _root;

	public BinaryTree() { }

	public BinaryTree(BinaryTreeNode root)
	{
		_root = root;
		_head = root;
		root.SetTree(this);
	}

	public virtual BinaryTreeNode? Root => _root;
	public virtual BinaryTreeNode? Head => _head;

	public bool IsEmpty => Root is null;

	public BinaryTreeNode MoveHeadTo(BinaryTreeNode node)
	{
		EnsureNodeIsInTree(node);
		_head = node;
		return node;
	}

	private void EnsureNodeIsInTree(BinaryTreeNode node)
	{
		if (node.Tree != this) // Hit root node in other tree
		{
			throw new NodeIsNotInBinaryTreeException();
		}
	}

	public virtual BinaryTreeNode SetRoot(BinaryTreeNode newRootNode)
	{
		_root?.ChangeParent(newRootNode, true);
		_root = newRootNode;
		_head = newRootNode;
		newRootNode.SetTree(this);
		return newRootNode;
	}

	/// <summary>
	///     Inserts new node at the head by making the head an orphan
	///     And then having the new adopt it as the left child
	/// </summary>
	/// <param name="newNode">New node to be inserted</param>
	/// <returns></returns>
	public BinaryTreeNode InsertNodeAtHead(BinaryTreeNode newNode)
	{
		if (Head is null)
		{
			throw new InvalidOperationException("Head is detached from tree");
		}
		if (Head.Parent is null)
		{
			throw new InvalidOperationException("Cannot replace root using this method");
		}

		BinaryTreeNode? headParent  = Head.Parent;
		bool            isLeftChild = Head.Parent.LeftChild == Head;
		Head.ChangeParent(newNode, true);
		newNode.ChangeParent(headParent, isLeftChild);
		MoveHeadTo(newNode);
		return newNode;
	}
}
