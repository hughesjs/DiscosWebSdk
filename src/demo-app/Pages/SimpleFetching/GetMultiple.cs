using System.Reflection;
using DISCOSweb_Sdk.Clients;
using DISCOSweb_Sdk.Models.ResponseModels;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Microsoft.AspNetCore.Components;

namespace DISCOSweb_demo_app.Pages.SimpleFetching;

public partial class GetMultiple
{
	[Inject]
	private IDiscosClient DiscosClient { get; set; }

	private Type TargetType { get; set; }
	
	private List<DiscosModelBase> Elements { get; set; }
	
	private bool _loading { get; set; }


	private Task UpdateModel(Type t)
	{
		
	}
}
