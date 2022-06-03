using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Queries.Filters;
using DISCOSweb_Sdk.Queries.Filters.Tree;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Queries.Filters.FilterTrees;

public class FilterTreeTests
{
	[Fact]
	public void NewTreeHasZeroNodes() // We don't count root node
	{
		FilterTree tree = new();
		tree.CountNodes().ShouldBe(0);
	}

	[Fact]
	public void CanCountTwoLayersOfNodes()
	{
		FilterTree tree = new();
		FilterNode andNode = tree.RootNode.AddChild(new OperationNode(NodeOperation.And));
		andNode.AddChild(new DefinitionNode(new FilterDefinition<DiscosObject,string>(nameof(DiscosObject.Name), "International", DiscosFunction.Contains)));
		andNode.AddChild(new DefinitionNode(new FilterDefinition<DiscosObject,float>(nameof(DiscosObject.Height), 0.5f, DiscosFunction.GreaterThan)));
		tree.CountNodes().ShouldBe(3);
	}

	[Fact]
	public void CanCountThreeLayersOfNodes()
	{
		FilterTree tree = new();
		FilterNode topAnd = tree.RootNode.AddChild(new OperationNode(NodeOperation.And));
		FilterNode l2Or = topAnd.AddChild(new OperationNode(NodeOperation.Or));
		FilterNode l2And = topAnd.AddChild(new OperationNode(NodeOperation.And));
		l2Or.AddChild(new OperationNode(NodeOperation.Or));
		l2Or.AddChild(new OperationNode(NodeOperation.Or));
		l2And.AddChild(new OperationNode(NodeOperation.And));
		l2And.AddChild(new OperationNode(NodeOperation.And));
		tree.CountNodes().ShouldBe(7);
	}
}
