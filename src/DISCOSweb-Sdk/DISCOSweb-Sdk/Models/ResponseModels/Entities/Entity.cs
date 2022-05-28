using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;

namespace DISCOSweb_Sdk.Models.ResponseModels.Entities;

public record Entity : DiscosModelBase
{
	[JsonPropertyName("launches")]
	public IReadOnlyList<Launch> Launches { get; init; } = new ReadOnlyCollection<Launch>(new List<Launch>());
    
	[JsonPropertyName("objects")]
	public IReadOnlyList<DiscosObject> Objects { get; init; } = new ReadOnlyCollection<DiscosObject>(new List<DiscosObject>());
    
	[JsonPropertyName("launchSystems")]
	public IReadOnlyList<LaunchSystem> LaunchSystems { get; init; }= new ReadOnlyCollection<LaunchSystem>(new List<LaunchSystem>());
    
	[JsonPropertyName("launchSites")]
	public IReadOnlyList<LaunchSite> LaunchSites { get; init; }= new ReadOnlyCollection<LaunchSite>(new List<LaunchSite>());

}
