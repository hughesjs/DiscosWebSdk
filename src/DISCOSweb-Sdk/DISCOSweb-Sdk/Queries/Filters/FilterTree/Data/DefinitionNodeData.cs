using DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

internal class DefinitionNodeData<TObject, TParam>: DefinitionNodeData where TObject : notnull
{
	public override FilterDefinition<TObject, TParam>? Value => base.Value as FilterDefinition<TObject, TParam>;
	public DefinitionNodeData(object? value) : base(value) { }
}


internal class DefinitionNodeData : FilterTreeNodeData
{
	public override FilterDefinition? Value => base.Value as FilterDefinition;
	public DefinitionNodeData(object? value) : base(value) { }
}
