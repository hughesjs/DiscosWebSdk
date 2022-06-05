using DISCOSweb_Sdk.DataStructures;
using DISCOSweb_Sdk.Exceptions.BinaryTree;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.DataStructures.BinaryTrees;

public abstract class BinaryTreeTests
{

	public class GenericTreeTests
	{
		
		
	}


	public class NonGenericTests
	{
		private DISCOSweb_Sdk.DataStructures.BinaryTrees.BinaryTree _tree;
		private readonly BinaryTreeNode _rootNode;

		public NonGenericTests()
		{
			_rootNode = new();
			_tree = new(_rootNode);
		}
		
		[Fact]
		public void SetsRootOnConstruct()
		{
			_tree.Root.ShouldBe(_rootNode);
		}

		[Fact]
		public void SetsHeadToRootOnConstruct()
		{
			_tree.Head.ShouldBe(_rootNode);
		}

		[Fact]
		public void ReturnsIsEmptyIfRootIsNull()
		{
			_tree = new();
			_tree.IsEmpty.ShouldBeTrue();
		}

		[Fact]
		public void ReturnsNotEmptyIfRootIsNotNull()
		{
			_tree.IsEmpty.ShouldBeFalse();
		}

		[Fact]
		public void CanSetRightChild()
		{
			BinaryTreeNode newNode = new();
			_rootNode.SetRightChild(newNode);
			_rootNode.RightChild.ShouldBe(newNode);
			newNode.Parent.ShouldBe(_rootNode);
		}

		[Fact]
		public void CanSetLeftChild()
		{
			BinaryTreeNode newNode = new();
			_rootNode.SetLeftChild(newNode);
			_rootNode.LeftChild.ShouldBe(newNode);
			newNode.Parent.ShouldBe(_rootNode);
		}

		[Fact]
		public void CanMoveHead()
		{
			BinaryTreeNode newNode = new();
			_tree.Root!.SetLeftChild(newNode);
			_tree.MoveHeadTo(newNode);
			_tree.Head.ShouldBe(newNode);
		}

		[Fact]
		public void ThrowsIfHeadMovedToNonTreeNode()
		{
			Should.Throw<NodeIsNotInBinaryTreeException>(() => _tree.MoveHeadTo(new()));
		}

		[Fact]
		public void CanSetRootOnEmptyTree()
		{
			_tree = new();
			_tree.Root.ShouldBeNull();
			_tree.SetRoot(_rootNode);
			_tree.Root.ShouldBe(_rootNode);
		}

		[Fact]
		public void HeadIsMovedOnSetRoot()
		{
			_tree = new();
			_tree.Root.ShouldBeNull();
			_tree.SetRoot(_rootNode);
			_tree.Head.ShouldBe(_rootNode);
		}

		[Fact]
		public void ThrowsIfRootSetMoreThanOnce()
		{
			Should.Throw<BinaryTreeAlreadyHasRootException>(() => _tree.SetRoot(new()));
		}
	}
}
