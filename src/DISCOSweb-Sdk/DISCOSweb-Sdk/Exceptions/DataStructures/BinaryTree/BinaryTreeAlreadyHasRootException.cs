namespace DISCOSweb_Sdk.Exceptions.DataStructures.BinaryTree;

public class BinaryTreeAlreadyHasRootException: Exception
{
	public BinaryTreeAlreadyHasRootException() : base("Binary tree already has a root, please create a new tree if you want a new root") { }
}
