using DISCOSweb_Sdk.Exceptions.DataStructures.BinaryTree;

namespace DISCOSweb_Sdk.DataStructures.BinaryTrees;

internal class BinaryTree<TNodeType> : BinaryTree where TNodeType: BinaryTreeNode
{
	public override TNodeType? Root => _root as TNodeType;
	public override TNodeType? Head => _head as TNodeType;
	
	
	public BinaryTree(){}
	public BinaryTree(TNodeType rootNode): base(rootNode) { }
	
	public override TNodeType SetRoot(BinaryTreeNode rootNode)
	{
		if (rootNode is not TNodeType node)
		{
			throw new InvalidNodeTypeException("Root must be of same type as tree.");
		}
		base.SetRoot(node);
		return node;
	}
}


internal class BinaryTree
{
	protected BinaryTreeNode? _root;
	protected BinaryTreeNode? _head;
	public virtual BinaryTreeNode? Root => _root;
	public virtual BinaryTreeNode? Head => _head;

	public BinaryTree() { }

	public BinaryTree(BinaryTreeNode root)
	{
		_root = root;
		_head = root;
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

	public virtual BinaryTreeNode SetRoot(BinaryTreeNode rootNode)
	{
		if (_root is not null)
		{
			throw new BinaryTreeAlreadyHasRootException();
		}
		_root = rootNode;
		_head = rootNode;
		rootNode.SetTree(this);
		return rootNode;
	}
}
