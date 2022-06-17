using DiscosWebSdk.Enums;
using DiscosWebSdk.Exceptions.Queries.Filters.FilterTree;
using DiscosWebSdk.Models.ResponseModels.DiscosObjects;
using DiscosWebSdk.Queries.Filters;
using DiscosWebSdk.Queries.Filters.FilterTree;
using DiscosWebSdk.Queries.Filters.FilterTree.Nodes;
using Shouldly;
using Xunit;

namespace DiscosWebSdk.Tests.Queries.Filters.FilterTrees;

public class FilterTreeTests
{
	private readonly FilterTree _tree;

	public FilterTreeTests() => _tree = new();

	[Fact]
	public void CanAddDefinitionToEmptyTree()
	{
		FilterDefinition definition = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "123", DiscosFunction.Equal);

		DefinitionNode defNode = _tree.AddDefinitionNode(definition);

		_tree.Root.ShouldBe(defNode);
		defNode.Tree.ShouldBe(_tree);
	}

	[Fact]
	public void ThrowsIfOperationNodeAddedToEmptyTree()
	{
		Should.Throw<InvalidFilterTreeException>(() => _tree.AddOperationNode(FilterOperation.And));
	}

	[Fact]
	public void CanAddFullOperation()
	{
		FilterDefinition f1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "123", DiscosFunction.Equal);
		FilterDefinition f2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "321", DiscosFunction.Equal);

		DefinitionNode leftOperand  = _tree.AddDefinitionNode(f1);
		OperationNode  opNode       = _tree.AddOperationNode(FilterOperation.And);
		DefinitionNode rightOperand = _tree.AddDefinitionNode(f2);

		_tree.Root.ShouldBe(opNode);
		opNode.LeftChild.ShouldBe(leftOperand);
		opNode.RightChild.ShouldBe(rightOperand);
		leftOperand.Parent.ShouldBe(opNode);
		rightOperand.Parent.ShouldBe(opNode);
	}

	[Fact]
	public void CanAddCompoundOperation()
	{
		FilterDefinition fd1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "1", DiscosFunction.Equal);
		FilterDefinition fd2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "2", DiscosFunction.Equal);
		FilterDefinition fd3 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "3", DiscosFunction.NotEqual);
		FilterDefinition fd4 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "4", DiscosFunction.NotEqual);

		FilterTreeNode f1 = _tree.AddDefinitionNode(fd1);
		OperationNode  a1 = _tree.AddOperationNode(FilterOperation.And);
		FilterTreeNode f2 = _tree.AddDefinitionNode(fd2);
		OperationNode  a2 = _tree.AddOperationNode(FilterOperation.Or);
		FilterTreeNode f3 = _tree.AddDefinitionNode(fd3);
		OperationNode  a3 = _tree.AddOperationNode(FilterOperation.And);
		FilterTreeNode f4 = _tree.AddDefinitionNode(fd4);

		_tree.Root.ShouldBe(a2);

		a2.LeftChild.ShouldBe(a1);
		a2.RightChild.ShouldBe(a3);

		a1.LeftChild.ShouldBe(f1);
		a1.RightChild.ShouldBe(f2);

		a3.LeftChild.ShouldBe(f3);
		a3.RightChild.ShouldBe(f4);
	}

	[Fact]
	public void CanCorrectlyDetermineIfTreeIsComplete()
	{
		FilterDefinition fd1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "1", DiscosFunction.Equal);
		FilterDefinition fd2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "2", DiscosFunction.Equal);
		FilterDefinition fd3 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "3", DiscosFunction.NotEqual);
		FilterDefinition fd4 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "4", DiscosFunction.NotEqual);

		_tree.AddDefinitionNode(fd1);
		_tree.IsCompleteTree().ShouldBeTrue();
		_tree.AddOperationNode(FilterOperation.And);
		_tree.IsCompleteTree().ShouldBeFalse();
		_tree.AddDefinitionNode(fd2);
		_tree.IsCompleteTree().ShouldBeTrue();
		_tree.AddOperationNode(FilterOperation.Or);
		_tree.IsCompleteTree().ShouldBeFalse();
		_tree.AddDefinitionNode(fd3);
		_tree.IsCompleteTree().ShouldBeTrue();
		_tree.AddOperationNode(FilterOperation.And);
		_tree.IsCompleteTree().ShouldBeFalse();
		_tree.AddDefinitionNode(fd4);
		_tree.IsCompleteTree().ShouldBeTrue();

		new FilterTree().IsCompleteTree().ShouldBeFalse();
	}

	[Fact]
	public void CanPrintSingleNodeTree()
	{
		_tree.AddDefinitionNode(new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "123", DiscosFunction.Equal));
		_tree.ToString().ShouldBe("eq(name,'123')");
	}

	[Fact]
	public void CanPrintSingleOperationTree()
	{
		FilterDefinition f1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "123", DiscosFunction.Equal);
		FilterDefinition f2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "321", DiscosFunction.Equal);

		_tree.AddDefinitionNode(f1);
		_tree.AddOperationNode(FilterOperation.And);
		_tree.AddDefinitionNode(f2);

		_tree.ToString().ShouldBe($"and({f1},{f2})");
	}

	[Fact]
	public void CanPrintMultipleOperationTree()
	{
		FilterDefinition fd1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "1", DiscosFunction.Equal);
		FilterDefinition fd2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "2", DiscosFunction.Equal);
		FilterDefinition fd3 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "3", DiscosFunction.NotEqual);
		FilterDefinition fd4 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "4", DiscosFunction.NotEqual);

		_tree.AddDefinitionNode(fd1);
		_tree.AddOperationNode(FilterOperation.And);
		_tree.AddDefinitionNode(fd2);
		_tree.AddOperationNode(FilterOperation.Or);
		_tree.AddDefinitionNode(fd3);
		_tree.AddOperationNode(FilterOperation.And);
		_tree.AddDefinitionNode(fd4);

		_tree.ToString().ShouldBe($"or(and({fd1},{fd2}),and({fd3},{fd4}))");
	}

	[Fact]
	public void CanPrintIncompleteTree()
	{
		FilterDefinition f1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "123", DiscosFunction.Equal);

		_tree.AddDefinitionNode(f1);
		_tree.AddOperationNode(FilterOperation.And);

		_tree.ToString().ShouldBe($"and({f1},)");
	}
}
