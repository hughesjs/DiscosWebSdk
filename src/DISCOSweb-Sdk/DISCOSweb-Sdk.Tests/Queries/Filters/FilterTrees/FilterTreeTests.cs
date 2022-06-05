using System;
using System.Linq;
using AutoFixture.Kernel;
using DISCOSweb_Sdk.Enums;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Queries.Filters;
using DISCOSweb_Sdk.Queries.Filters.FilterTree;
using DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;
using Shouldly;
using Xunit;

namespace DISCOSweb_Sdk.Tests.Queries.Filters.FilterTrees;

public class FilterTreeTests
{
	// [Fact]
	// public void NewTreeHasZeroNodes() // We don't count root node
	// {
	// 	FilterTree tree = new();
	// 	tree.CountNodes().ShouldBe(0);
	// }
	//
	// [Fact]
	// public void CanCountTwoLayersOfNodes()
	// {
	// 	FilterTree tree = new();
	// 	FilterNode andNode = tree.Root.AddChild(new OperationNode(FilterOperation.And));
	// 	andNode.AddChild(new DefinitionNode(new FilterDefinition<DiscosObject,string>(nameof(DiscosObject.Name), "International", DiscosFunction.Contains)));
	// 	andNode.AddChild(new DefinitionNode(new FilterDefinition<DiscosObject,float>(nameof(DiscosObject.Height), 0.5f, DiscosFunction.GreaterThan)));
	// 	tree.CountNodes().ShouldBe(3);
	// }
	//
	// [Fact]
	// public void CanCountThreeLayersOfNodes()
	// {
	// 	FilterTree tree = new();
	// 	FilterNode topAnd = tree.Root.AddChild(new OperationNode(FilterOperation.And));
	// 	FilterNode l2Or = topAnd.AddChild(new OperationNode(FilterOperation.Or));
	// 	FilterNode l2And = topAnd.AddChild(new OperationNode(FilterOperation.And));
	// 	l2Or.AddChild(new OperationNode(FilterOperation.Or));
	// 	l2Or.AddChild(new OperationNode(FilterOperation.Or));
	// 	l2And.AddChild(new OperationNode(FilterOperation.And));
	// 	l2And.AddChild(new OperationNode(FilterOperation.And));
	// 	tree.CountNodes().ShouldBe(7);
	// }
	//
	// [Fact]
	// public void CanBuildSingleOperationTree()
	// {
	// 	FilterTree tree = new();
	// 	FilterDefinition left = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "123", DiscosFunction.Equal);
	// 	FilterDefinition right = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "321", DiscosFunction.NotEqual);
	// 	tree.AddFilter(left);
	// 	tree.AddAnd();
	// 	tree.AddFilter(right);
	// 	
	// 	tree.CountNodes().ShouldBe(3);
	// 	tree.Root.Children.First().ShouldBeOfType<OperationNode>();
	// 	tree.Root.Children.First().Children.ShouldAllBe(node => node is DefinitionNode);
	// }
	//
	// [Fact]
	// public void CanAddSingleDefinitionToRoot()
	// {
	// 	FilterTree tree = new();
	// 	tree.AddFilter(new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "123", DiscosFunction.Equal));
	// }
	//
	// [Fact]
	// public void CanBuildSingleCompoundNodeTree()
	// {
	// 	FilterTree tree = new();
	// 	FilterDefinition left1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "123", DiscosFunction.Equal);
	// 	FilterDefinition right1 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "321", DiscosFunction.NotEqual);
	// 	FilterDefinition left2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Id), "333", DiscosFunction.Equal);
	// 	FilterDefinition right2 = new FilterDefinition<DiscosObject, string>(nameof(DiscosObject.Name), "666", DiscosFunction.NotEqual);
	// 	tree.AddFilter(left1);
	// 	tree.AddOr();
	// 	tree.AddFilter(right1);
	// 	tree.AddAnd();
	// 	tree.AddFilter(left2);
	// 	tree.AddOr();
	// 	tree.AddFilter(right2);
	//
	// 	
	//
	// }

}
