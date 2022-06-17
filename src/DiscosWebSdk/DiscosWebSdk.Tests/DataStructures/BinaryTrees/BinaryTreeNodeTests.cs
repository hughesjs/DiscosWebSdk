using DiscosWebSdk.DataStructures.BinaryTrees;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.DataStructures.BinaryTrees;

public abstract class BinaryTreeNodeTests
{
	[Fact]
	public abstract void CanSetRightChild();

	[Fact]
	public abstract void CanSetLeftChild();

	[Fact]
	public abstract void SettingRightChildSetsTreeRef();

	[Fact]
	public abstract void SettingLeftChildSetsTreeRef();


	public class GenericTests : BinaryTreeNodeTests
	{
		private readonly BinaryTreeNode<string>             _node;
		private readonly BinaryTree<BinaryTreeNode<string>> _tree;

		public GenericTests()
		{
			_tree = new();
			_node = new();
			_tree.SetRoot(_node);
		}

		public override void CanSetRightChild()
		{
			BinaryTreeNode<string> newNode = new();
			_node.SetRightChild(newNode);
			_node.RightChild.ShouldBe(newNode);
			newNode.Parent.ShouldBe(_node);
		}

		public override void CanSetLeftChild()
		{
			BinaryTreeNode<string> newNode = new();
			_node.SetLeftChild(newNode);
			_node.LeftChild.ShouldBe(newNode);
			newNode.Parent.ShouldBe(_node);
		}

		public override void SettingRightChildSetsTreeRef()
		{
			BinaryTreeNode<string> newNode = new();
			_node.SetRightChild(newNode);
			_node.RightChild!.Tree.ShouldBe(_tree);
		}

		public override void SettingLeftChildSetsTreeRef()
		{
			BinaryTreeNode<string> newNode = new();
			_node.SetLeftChild(newNode);
			_node.LeftChild!.Tree.ShouldBe(_tree);
		}
	}


	public class NonGenericTests : BinaryTreeNodeTests
	{
		private readonly BinaryTreeNode _node;
		private readonly BinaryTree     _tree;

		public NonGenericTests()
		{
			_tree = new();
			_node = new();
			_tree.SetRoot(_node);
		}

		public override void CanSetRightChild()
		{
			BinaryTreeNode newNode = new();
			_node.SetRightChild(newNode);
			_node.RightChild.ShouldBe(newNode);
			newNode.Parent.ShouldBe(_node);
		}

		public override void CanSetLeftChild()
		{
			BinaryTreeNode newNode = new();
			_node.SetLeftChild(newNode);
			_node.LeftChild.ShouldBe(newNode);
			newNode.Parent.ShouldBe(_node);
		}

		public override void SettingRightChildSetsTreeRef()
		{
			BinaryTreeNode newNode = new();
			_node.SetRightChild(newNode);
			_node.RightChild!.Tree.ShouldBe(_tree);
		}

		public override void SettingLeftChildSetsTreeRef()
		{
			BinaryTreeNode newNode = new();
			_node.SetLeftChild(newNode);
			_node.LeftChild!.Tree.ShouldBe(_tree);
		}
	}
}
