namespace DISCOSweb_Sdk.Exceptions.DataStructures.BinaryTree;

public class NodeIsNotInBinaryTreeException: Exception
{
	public NodeIsNotInBinaryTreeException() : base("Cannot set head to node in different tree") {}
}
