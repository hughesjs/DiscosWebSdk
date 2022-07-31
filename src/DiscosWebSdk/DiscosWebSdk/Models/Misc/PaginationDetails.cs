using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace DiscosWebSdk.Models.Misc;

public class PaginationDetails
{
	[JsonPropertyName("totalPages")]
	public int TotalPages { get; [UsedImplicitly] init; } = -1;
	
	[JsonPropertyName("currentPage")]
	public int CurrentPage { get; [UsedImplicitly] init; } = -1;
	
	[JsonPropertyName("pageSize")]
	public int PageSize { get; [UsedImplicitly] init; } = -1;
}
