using DISCOSweb_Sdk.Queries.Filters.FilterTree.Nodes;

namespace DISCOSweb_Sdk.Queries.Filters.FilterTree.Data;

internal class DefinitionNodeData<TObject, TParam>: FilterTreeNodeData
{
	public DefinitionNode<TObject, TParam> Value { get; set; }
}
