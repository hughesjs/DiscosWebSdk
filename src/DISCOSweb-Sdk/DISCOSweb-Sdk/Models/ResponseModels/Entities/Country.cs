using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.Launches;

namespace DISCOSweb_Sdk.Models.ResponseModels.Entities;

public record Country: DiscosModelBase
{
	[JsonPropertyName("launches")]
	public IReadOnlyCollection<Launch> Launches { get; init; } = new ReadOnlyCollection<Launch>(new List<Launch>());
    
	[JsonPropertyName("objects")]
	public IReadOnlyCollection<DiscosObject> Objects { get; init; } = new ReadOnlyCollection<DiscosObject>(new List<DiscosObject>());
    
	[JsonPropertyName("launchSystems")]
	public IReadOnlyCollection<LaunchSystem> LaunchSystems { get; init; }= new ReadOnlyCollection<LaunchSystem>(new List<LaunchSystem>());
    
	[JsonPropertyName("launchSites")]
	public IReadOnlyCollection<LaunchSite> LaunchSites { get; init; }= new ReadOnlyCollection<LaunchSite>(new List<LaunchSite>());
}
