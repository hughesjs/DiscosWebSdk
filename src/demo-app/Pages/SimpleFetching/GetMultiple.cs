using System.Reflection;
using DISCOSweb_Sdk.Clients;
using DISCOSweb_Sdk.Models.ResponseModels;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using DISCOSweb_Sdk.Models.ResponseModels.FragmentationEvent;
using Microsoft.AspNetCore.Components;

namespace DISCOSweb_demo_app.Pages.SimpleFetching;

public partial class GetMultiple
{
	[Inject]
	private IDiscosClient DiscosClient { get; set; }

	private IReadOnlyList<FragmentationEvent> FragmentationEvents { get; set; }
	
	private bool _loading { get; set; }

	private async Task UpdateModel()
	{
		_loading            = true;
		FragmentationEvents = await DiscosClient.GetMultiple<FragmentationEvent>();
		_loading            = false;
	}
}
