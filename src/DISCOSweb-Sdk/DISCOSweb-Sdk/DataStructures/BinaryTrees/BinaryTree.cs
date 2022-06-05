using DISCOSweb_Sdk.Exceptions.BinaryTree;

namespace DISCOSweb_Sdk.DataStructures.BinaryTrees;

internal class BinaryTree<T> : BinaryTree
{
	public override BinaryTreeNode<T>? Root { get;  }
	public override BinaryTreeNode<T>? Head { get; }
	
	public override BinaryTreeNode<T> SetRoot(BinaryTreeNode rootNode)
	{
		if (Root is not null)
		{
			throw new BinaryTreeAlreadyHasRootException();
		}
		if (rootNode is not BinaryTreeNode<T> node)
		{
			throw new InvalidNodeTypeException("Root must be of same type as tree.");
		}
		BaseSetRoot(node);
		return node;
	}
}


internal abstract class BinaryTree
{
	private BinaryTreeNode? _root;
	private BinaryTreeNode? _head;
	public virtual BinaryTreeNode? Root => _root;
	public virtual BinaryTreeNode? Head => _head;

	public BinaryTree() { }

	public BinaryTree(BinaryTreeNode root)
	{
		BaseSetRoot(root);
	}

	public bool IsEmpty => Root is null;
	public BinaryTreeNode MoveHeadTo(BinaryTreeNode node)
	{
		EnsureNodeIsInTree(node);
		_head = node;
		return node;
	}

	private void EnsureNodeIsInTree(BinaryTreeNode node)
	{
		if (node == Root)
		{
			return;
		}
		if (node.Parent is null) // Hit root node in other tree
		{
			throw new NodeIsNotInBinaryTreeException(); 
		}
		EnsureNodeIsInTree(node.Parent);
	}

	protected void BaseSetRoot(BinaryTreeNode newRoot)
	{
		_root = newRoot;
		_head = newRoot;
	} 
	public abstract BinaryTreeNode SetRoot(BinaryTreeNode rootNode);
}
