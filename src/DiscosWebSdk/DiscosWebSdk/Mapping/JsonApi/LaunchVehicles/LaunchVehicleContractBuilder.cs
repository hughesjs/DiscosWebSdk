using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.Launches;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.LaunchVehicles;

internal static class LaunchVehicleContractBuilder
{
	internal static DelegatingContractBuilder<LaunchVehicle> WithLaunchVehicle(this IBuilder builder)
	{
		const string launchVehicleIdField = nameof(LaunchVehicle.Id);
		const string familyLinkTemplate   = $"/api/launch-vehicles/{launchVehicleIdField}/family";
		const string stageLinkTemplate    = $"/api/launch-vehicles/{launchVehicleIdField}/stages";
		const string launchLinkTemplate   = $"/api/launch-vehicles/{launchVehicleIdField}/launches";
		const string engineLinkTemplate   = $"/api/launch-vehicles/{launchVehicleIdField}/engines";

		object IdSelector(LaunchVehicle vehicle) => vehicle.Id;

		return builder.With<LaunchVehicle>("vehicle")
					  .Id(nameof(LaunchVehicle.Id))
					  .BelongsTo<LaunchVehicleFamily>(AttributeUtilities.GetJsonPropertyName<LaunchVehicle>(nameof(LaunchVehicle.Family)))
					  .Template(familyLinkTemplate, launchVehicleIdField, IdSelector)
					  .HasMany<LaunchVehicleStage>(AttributeUtilities.GetJsonPropertyName<LaunchVehicle>(nameof(LaunchVehicle.Stages)))
					  .Template(stageLinkTemplate, launchVehicleIdField, IdSelector)
					  .HasMany<Launch>(AttributeUtilities.GetJsonPropertyName<LaunchVehicle>(nameof(LaunchVehicle.Launches)))
					  .Template(launchLinkTemplate, launchVehicleIdField, IdSelector)
					  .HasMany<LaunchVehicleEngine>(AttributeUtilities.GetJsonPropertyName<LaunchVehicle>(nameof(LaunchVehicle.Engines)))
					  .Template(engineLinkTemplate, launchVehicleIdField, IdSelector);
	}
}
