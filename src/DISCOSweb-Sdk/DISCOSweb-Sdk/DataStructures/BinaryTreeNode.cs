namespace DISCOSweb_Sdk.DataStructures;

internal abstract class BinaryTreeNode<T>: BinaryTreeNode
{
	protected BinaryTreeNode(BinaryTreeNode<T>? parent, T? data)
	{
		Data = data;
		Parent = parent;
	}

	public virtual T? Data { get; }

	public new BinaryTreeNode<T>? Parent
	{
		get => base.Parent as BinaryTreeNode<T>;
		set => base.Parent = value;
	}
	public new BinaryTreeNode<T>? LeftChild
	{
		get => base.LeftChild as BinaryTreeNode<T>;
		set => base.LeftChild = value;
	}
	public new BinaryTreeNode<T>? RightChild 
	{
		get => base.RightChild as BinaryTreeNode<T>;
		set => base.RightChild = value;
	}
}


internal abstract class BinaryTreeNode
{
	public BinaryTreeNode? Parent { get; set; }
	public BinaryTreeNode? LeftChild { get; set; }
	public BinaryTreeNode? RightChild { get; set; }
}
