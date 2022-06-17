using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using DiscosWebSdk.Models.ResponseModels.Propellants;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.LaunchVehicles;

internal static class LaunchVehicleStageContractBuilder
{
	internal static DelegatingContractBuilder<LaunchVehicleStage> WithLaunchVehicleStage(this IBuilder builder)
	{
		const string stageIdFieldName       = nameof(LaunchVehicleStage.Id);
		const string propellantLinkTemplate = $"/api/launch-vehicles/stages/{stageIdFieldName}/propellant";
		const string vehiclesLinkTemplate   = $"/api/launch-vehicles/stages/{stageIdFieldName}/vehicles";

		object IdSelector(LaunchVehicleStage stage) => stage.Id;

		return builder.With<LaunchVehicleStage>("stage")
					  .Id(nameof(LaunchVehicleStage.Id))
					  .BelongsTo<Propellant>(AttributeUtilities.GetJsonPropertyName<LaunchVehicleStage>(nameof(LaunchVehicleStage.Propellant)))
					  .Template(propellantLinkTemplate, stageIdFieldName, IdSelector)
					  .HasMany<LaunchVehicle>(AttributeUtilities.GetJsonPropertyName<LaunchVehicleStage>(nameof(LaunchVehicleStage.Vehicles)))
					  .Template(vehiclesLinkTemplate, stageIdFieldName, IdSelector);
	}
}
