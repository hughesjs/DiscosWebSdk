using System;
using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Exceptions.BinaryTree;
using JetBrains.Annotations;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.DataStructures.BinaryTrees;

public abstract class BinaryTreeTests
{
	[Fact]
	public abstract void SetsRootOnConstruct();

	[Fact]
	public abstract void SetsHeadToRootOnConstruct();

	[Fact]
	public abstract void ReturnsIsEmptyIfRootIsNull();

	[Fact]
	public abstract void ReturnsNotEmptyIfRootIsNotNull();

	[Fact]
	public abstract void CanMoveHead();

	[Fact]
	public abstract void ThrowsIfHeadMovedToNonTreeNode();

	[Fact]
	public abstract void CanSetRootOnEmptyTree();

	[Fact]
	public abstract void HeadIsMovedOnSetRoot();

	[Fact]
	public abstract void ThrowsIfRootSetMoreThanOnce();

	[Fact]
	public abstract void SetRootSetsTreeRef();


	[UsedImplicitly]
	public class GenericTreeTests : BinaryTreeTests
	{
		private BinaryTree<SealedTestNodeType> _tree;
		private readonly SealedTestNodeType _rootNode;

		public GenericTreeTests()
		{
			_rootNode = new();
			_tree = new(_rootNode);
		}

		public override void SetsRootOnConstruct()
		{
			_tree.Root.ShouldBe(_rootNode);
		}

		public override void SetsHeadToRootOnConstruct()
		{
			_tree.Head.ShouldBe(_rootNode);
		}

		public override void ReturnsIsEmptyIfRootIsNull()
		{
			_tree = new();
			_tree.IsEmpty.ShouldBeTrue();
		}

		public override void ReturnsNotEmptyIfRootIsNotNull()
		{
			_tree.IsEmpty.ShouldBeFalse();
		}

		public override void CanMoveHead()
		{
			SealedTestNodeType newNode = new();
			_tree.Root!.SetLeftChild(newNode);
			_tree.MoveHeadTo(newNode);
			_tree.Head.ShouldBe(newNode);
		}

		public override void ThrowsIfHeadMovedToNonTreeNode()
		{
			Should.Throw<NodeIsNotInBinaryTreeException>(() => _tree.MoveHeadTo(new()));
		}

		public override void CanSetRootOnEmptyTree()
		{
			_tree = new();
			_tree.Root.ShouldBeNull();
			_tree.SetRoot(_rootNode);
			_tree.Root.ShouldBe(_rootNode);
		}

		public override void HeadIsMovedOnSetRoot()
		{
			_tree = new();
			_tree.Root.ShouldBeNull();
			_tree.SetRoot(_rootNode);
			_tree.Head.ShouldBe(_rootNode);
		}

		public override void ThrowsIfRootSetMoreThanOnce()
		{
			Should.Throw<BinaryTreeAlreadyHasRootException>(() => _tree.SetRoot(new SealedTestNodeType()));
		}

		public override void SetRootSetsTreeRef()
		{
			_tree = new();
			_tree.Root.ShouldBeNull();
			_tree.SetRoot(_rootNode);
			_rootNode.Tree.ShouldBe(_tree);
		}

		[Fact]
		public void ThrowsWhenSettingRootIfNodeIsOfWrongType()
		{
			
			BinaryTree<SealedTestNodeType> freshTree = new();
			BinaryTreeNode<string> definitionNode = new();
			Should.Throw<InvalidNodeTypeException>(() => freshTree.SetRoot(definitionNode));
		}

		[Theory]
		[InlineData(typeof(TestNodeTypeA))]
		[InlineData(typeof(TestNodeTypeB))]
		[InlineData(typeof(TestNodeTypeC))]
		public void DoesntThrowWhenSettingRootToDerivedTypes(Type t)
		{
			BinaryTree<TestNodeTypeA> freshTree = new();
			TestNodeTypeA root = Activator.CreateInstance(t) as TestNodeTypeA ?? throw new("Whoops");
			Should.NotThrow(() => freshTree.SetRoot(root));
		}
		
		private sealed class SealedTestNodeType: BinaryTreeNode<string>{}

		private class TestNodeTypeA : BinaryTreeNode<object> {}
		private sealed class TestNodeTypeB : TestNodeTypeA {}
		private sealed class TestNodeTypeC : TestNodeTypeA {}

	}

	[UsedImplicitly]
	public class NonGenericTests: BinaryTreeTests
	{
		private BinaryTree _tree;
		private readonly BinaryTreeNode _rootNode;

		public NonGenericTests()
		{
			_rootNode = new();
			_tree = new(_rootNode);
		}

		public override void SetsRootOnConstruct()
		{
			_tree.Root.ShouldBe(_rootNode);
		}
		
		public override void SetsHeadToRootOnConstruct()
		{
			_tree.Head.ShouldBe(_rootNode);
		}

		public override void ReturnsIsEmptyIfRootIsNull()
		{
			_tree = new();
			_tree.IsEmpty.ShouldBeTrue();
		}

		public override void ReturnsNotEmptyIfRootIsNotNull()
		{
			_tree.IsEmpty.ShouldBeFalse();
		}

		public override void CanMoveHead()
		{
			BinaryTreeNode newNode = new();
			_tree.Root!.SetLeftChild(newNode);
			_tree.MoveHeadTo(newNode);
			_tree.Head.ShouldBe(newNode);
		}
		
		public override void ThrowsIfHeadMovedToNonTreeNode()
		{
			Should.Throw<NodeIsNotInBinaryTreeException>(() => _tree.MoveHeadTo(new()));
		}
		
		public override void CanSetRootOnEmptyTree()
		{
			_tree = new();
			_tree.Root.ShouldBeNull();
			_tree.SetRoot(_rootNode);
			_tree.Root.ShouldBe(_rootNode);
		}
		
		public override void HeadIsMovedOnSetRoot()
		{
			_tree = new();
			_tree.Root.ShouldBeNull();
			_tree.SetRoot(_rootNode);
			_tree.Head.ShouldBe(_rootNode);
		}
		
		public override void ThrowsIfRootSetMoreThanOnce()
		{
			Should.Throw<BinaryTreeAlreadyHasRootException>(() => _tree.SetRoot(new()));
		}

		public override void SetRootSetsTreeRef()
		{
			_tree = new();
			_tree.Root.ShouldBeNull();
			_tree.SetRoot(_rootNode);
			_rootNode.Tree.ShouldBe(_tree);
		}
	}
}
