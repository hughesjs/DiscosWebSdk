namespace DISCOSweb_Sdk.DataStructures;

internal abstract class BinaryTreeNode<T>
{
	protected BinaryTreeNode(BinaryTreeNode<T>? parent, T? data)
	{
		Data = data;
		Parent = parent;
	}

	public T? Data { get; set; }
	public BinaryTreeNode<T> Parent { get; set; }
	public BinaryTreeNode<T>? LeftChild { get; set; }
	public BinaryTreeNode<T>? RightChild { get; set; }
}
