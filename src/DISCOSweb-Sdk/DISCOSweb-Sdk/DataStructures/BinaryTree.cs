using DISCOSweb_Sdk.Exceptions.BinaryTree;

namespace DISCOSweb_Sdk.DataStructures;

internal class BinaryTree<T>
{
	public BinaryTreeNode<T>? Root { get; private set; }
	public BinaryTreeNode<T>? Head { get; private set; }

	public BinaryTree() { }

	public BinaryTree(BinaryTreeNode<T> root)
	{
		SetRoot(root);
	}

	public bool IsEmpty => Root is null;
	public BinaryTreeNode<T> MoveHeadTo(BinaryTreeNode<T> node)
	{
		EnsureNodeIsInTree(node);
		Head = node;
		return node;
	}

	private void EnsureNodeIsInTree(BinaryTreeNode<T> node)
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

	public BinaryTreeNode<T> SetRoot(BinaryTreeNode<T> rootNode)
	{
		if (Root is not null)
		{
			throw new BinaryTreeAlreadyHasRootException();
		}
		Root = rootNode;
		Head = rootNode;
		return rootNode;
	}
}
