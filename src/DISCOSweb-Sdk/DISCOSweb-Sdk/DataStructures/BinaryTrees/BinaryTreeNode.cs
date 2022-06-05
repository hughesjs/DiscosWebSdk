namespace DISCOSweb_Sdk.DataStructures;

internal abstract class BinaryTreeNode<T>: BinaryTreeNode
{
	protected BinaryTreeNode(BinaryTreeNode<T>? parent, T? data)
	{
		Data = data;
		Parent = parent;
	}

	public virtual T? Data { get; }

	public override BinaryTreeNode<T>? Parent { get; }
	public override BinaryTreeNode<T>? LeftChild { get; }
	public override BinaryTreeNode<T>? RightChild { get; }

	public override BinaryTreeNode<T> SetLeftChild(BinaryTreeNode node) => (BinaryTreeNode<T>)base.SetLeftChild(node);
	public override BinaryTreeNode<T> SetRightChild(BinaryTreeNode node) => (BinaryTreeNode<T>)base.SetRightChild(node);
}


internal class BinaryTreeNode
{
	private BinaryTreeNode? _parent, _leftChild, _rightChild;
	public virtual BinaryTreeNode? Parent => _parent;
	public virtual BinaryTreeNode? LeftChild => _leftChild;
	public virtual BinaryTreeNode? RightChild => _rightChild;

	public virtual BinaryTreeNode SetLeftChild(BinaryTreeNode node)
	{
		_leftChild = node;
		node.SetParent(this);
		return node;
	}

	public virtual BinaryTreeNode SetRightChild(BinaryTreeNode node)
	{
		_rightChild = node;
		node.SetParent(this);
		return node;
	}

	protected void SetParent(BinaryTreeNode node)
	{
		_parent = node;
	}
}
