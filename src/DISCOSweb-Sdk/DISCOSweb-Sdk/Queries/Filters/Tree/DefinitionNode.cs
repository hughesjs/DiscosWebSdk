namespace DISCOSweb_Sdk.Queries.Filters.Tree;

public class DefinitionNode: FilterNode
{
	public DefinitionNode(FilterDefinition definition) => Definition = definition;
	public FilterDefinition Definition { get; }
}
