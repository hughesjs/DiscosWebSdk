namespace DiscosWebSdk.Queries.Filters.FilterTree.Data;

internal class DefinitionNodeData<TObject, TParam> : FilterTreeNodeData<FilterDefinition<TObject, TParam>> where TObject : notnull
{
	public DefinitionNodeData(FilterDefinition<TObject, TParam> value) : base(value) { }
}


internal class DefinitionNodeData : FilterTreeNodeData<FilterDefinition>
{
	public DefinitionNodeData(FilterDefinition value) : base(value) { }
}
