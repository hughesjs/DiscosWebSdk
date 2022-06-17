using DiscosWebSdk.Misc;
using DiscosWebSdk.Models.ResponseModels.Launches;
using DiscosWebSdk.Models.ResponseModels.LaunchVehicles;
using Hypermedia.Configuration;

namespace DiscosWebSdk.Mapping.JsonApi.LaunchVehicles;

internal static class LaunchVehicleFamilyContractBuilder
{
	internal static DelegatingContractBuilder<LaunchVehicleFamily> WithLaunchVehicleFamily(this IBuilder builder)
	{
		const string vehicleFamilyIdField = nameof(LaunchVehicleFamily.Id);
		const string systemLinkTemplate   = $"/api/launch-vehicles/families/{vehicleFamilyIdField}/system";
		const string vehicleLinkTemplate  = $"/api/launch-vehicles/families/{vehicleFamilyIdField}/vehicles";
		object IdSelector(LaunchVehicleFamily family) => family.Id;

		return builder.With<LaunchVehicleFamily>("vehicleFamily")
					  .Id(nameof(LaunchVehicleFamily.Id))
					  .BelongsTo<LaunchSystem>(AttributeUtilities.GetJsonPropertyName<LaunchVehicleFamily>(nameof(LaunchVehicleFamily.System)))
					  .Template(systemLinkTemplate, vehicleFamilyIdField, IdSelector)
					  .HasMany<LaunchVehicle>(AttributeUtilities.GetJsonPropertyName<LaunchVehicleFamily>(nameof(LaunchVehicleFamily.Vehicles)))
					  .Template(vehicleLinkTemplate, vehicleFamilyIdField, IdSelector);
	}
}
