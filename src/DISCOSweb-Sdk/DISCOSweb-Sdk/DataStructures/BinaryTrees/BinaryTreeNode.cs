namespace DISCOSweb_Sdk.DataStructures.BinaryTrees;

internal class BinaryTreeNode<T> : BinaryTreeNode where T : class
{
	public BinaryTreeNode(BinaryTree tree, BinaryTreeNode<T>? parent, T? data): base(tree)
	{
		_data = data;
		_parent = parent;
	}

	public BinaryTreeNode() : base(){}

	public override T? Data => _data as T;

	public override BinaryTreeNode<T>? Parent => _parent as BinaryTreeNode<T>;
	public override BinaryTreeNode<T>? LeftChild => _leftChild as BinaryTreeNode<T>;
	public override BinaryTreeNode<T>? RightChild => _rightChild as BinaryTreeNode<T>;

	public override BinaryTreeNode<T> SetLeftChild(BinaryTreeNode node) => (BinaryTreeNode<T>)base.SetLeftChild(node);
	public override BinaryTreeNode<T> SetRightChild(BinaryTreeNode node) => (BinaryTreeNode<T>)base.SetRightChild(node);
}


internal class BinaryTreeNode
{

	public BinaryTreeNode(BinaryTree? tree)
	{
		Tree = tree;
	}
	
	public BinaryTreeNode(){}

	protected object? _data;

	protected BinaryTreeNode? _parent, _leftChild, _rightChild;
	public virtual BinaryTreeNode? Parent => _parent;
	public virtual BinaryTreeNode? LeftChild => _leftChild;
	public virtual BinaryTreeNode? RightChild => _rightChild;
	
	public BinaryTree? Tree { get; private set; }

	public virtual object? Data => _data;

	public virtual BinaryTreeNode SetLeftChild(BinaryTreeNode node)
	{
		_leftChild = node;
		node.SetParent(this);
		node.SetTree(Tree);
		return node;
	}

	public virtual BinaryTreeNode SetRightChild(BinaryTreeNode node)
	{
		_rightChild = node;
		node.SetParent(this);
		node.SetTree(Tree);
		return node;
	}

	protected void SetParent(BinaryTreeNode? newParent)
	{
		_parent = newParent;
	}

	public void ChangeParent(BinaryTreeNode? newParent, bool isLeft)
	{
		// Detach from old parent
		Parent?.AbandonChild(this);
		// Attach to new parent
		if (isLeft)
		{
			newParent?.SetLeftChild(this);
		}
		else
		{
			newParent?.SetRightChild(this);
		}
	}

	private void AbandonChild(BinaryTreeNode child)
	{
		if (_leftChild == child)
		{
			_leftChild = null;
		}
		if (_rightChild == child)
		{
			_rightChild = null;
		}
	}

	public void SetTree(BinaryTree? tree)
	{
		Tree = tree;
	}
}
