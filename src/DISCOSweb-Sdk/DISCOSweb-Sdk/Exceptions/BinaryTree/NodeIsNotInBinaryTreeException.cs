namespace DISCOSweb_Sdk.Exceptions.BinaryTree;

public class NodeIsNotInBinaryTreeException: Exception
{
	public NodeIsNotInBinaryTreeException() : base("Cannot set head to node in different tree") {}
}
