using System;
using DISCOSweb_Sdk.DataStructures.BinaryTrees;
using DISCOSweb_Sdk.Exceptions.DataStructures.BinaryTree;
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
	public abstract void AddsRootNodeAsNewLayerIfAlreadyHasRoot();

	[Fact]
	public abstract void SetRootSetsTreeRef();


	[UsedImplicitly]
	public class GenericTreeTests : BinaryTreeTests
	{
		private readonly SealedTestNodeType             _rootNode;
		private          BinaryTree<SealedTestNodeType> _tree;

		public GenericTreeTests()
		{
			_rootNode = new();
			_tree     = new(_rootNode);
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

		public override void AddsRootNodeAsNewLayerIfAlreadyHasRoot()
		{
			SealedTestNodeType oldRoot = _rootNode;
			SealedTestNodeType newRoot = new();
			_tree.SetRoot(newRoot);

			newRoot.LeftChild.ShouldBe(oldRoot);
			oldRoot.Parent.ShouldBe(newRoot);
			_tree.Head.ShouldBe(newRoot);
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

			BinaryTree<SealedTestNodeType> freshTree      = new();
			BinaryTreeNode<string>         definitionNode = new();
			Should.Throw<InvalidNodeTypeException>(() => freshTree.SetRoot(definitionNode));
		}

		[Theory]
		[InlineData(typeof(TestNodeTypeA))]
		[InlineData(typeof(TestNodeTypeB))]
		[InlineData(typeof(TestNodeTypeC))]
		public void DoesntThrowWhenSettingRootToDerivedTypes(Type t)
		{
			BinaryTree<TestNodeTypeA> freshTree = new();
			TestNodeTypeA             root      = Activator.CreateInstance(t) as TestNodeTypeA ?? throw new("Whoops");
			Should.NotThrow(() => freshTree.SetRoot(root));
		}

		private sealed class SealedTestNodeType : BinaryTreeNode<string> { }

		private class TestNodeTypeA : BinaryTreeNode<object> { }

		private sealed class TestNodeTypeB : TestNodeTypeA { }

		private sealed class TestNodeTypeC : TestNodeTypeA { }
	}


	[UsedImplicitly]
	public class NonGenericTests : BinaryTreeTests
	{
		private readonly BinaryTreeNode _rootNode;
		private          BinaryTree     _tree;

		public NonGenericTests()
		{
			_rootNode = new();
			_tree     = new(_rootNode);
		}

		[Fact]
		public void CanInsertNewNodeAtHead()
		{
			BinaryTreeNode newChildNode           = _rootNode.SetLeftChild(new());
			BinaryTreeNode newLeftGrandChildNode  = newChildNode.SetLeftChild(new());
			BinaryTreeNode newRightGrandChildNode = newChildNode.SetRightChild(new());
			_tree.MoveHeadTo(newChildNode);

			BinaryTreeNode interloperNode = _tree.InsertNodeAtHead(new());

			_rootNode.LeftChild.ShouldBe(interloperNode);
			interloperNode.LeftChild.ShouldBe(newChildNode);
			interloperNode.Parent.ShouldBe(_rootNode);
			newChildNode.Parent.ShouldBe(interloperNode);
			newChildNode.LeftChild.ShouldBe(newLeftGrandChildNode);
			newChildNode.RightChild.ShouldBe(newRightGrandChildNode);
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

		public override void AddsRootNodeAsNewLayerIfAlreadyHasRoot()
		{
			BinaryTreeNode oldRoot = _rootNode;
			BinaryTreeNode newRoot = new();
			_tree.SetRoot(newRoot);

			newRoot.LeftChild.ShouldBe(oldRoot);
			oldRoot.Parent.ShouldBe(newRoot);
			_tree.Head.ShouldBe(newRoot);
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
