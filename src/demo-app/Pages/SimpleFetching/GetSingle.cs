using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using DISCOSweb_Sdk.Clients;
using DISCOSweb_Sdk.Models.ResponseModels.DiscosObjects;
using Microsoft.AspNetCore.Components;

namespace DISCOSweb_demo_app.Pages.SimpleFetching;

public partial class GetSingle
{
    [Inject] private IDiscosClient _client { get; set; }

    [Inject] private HttpClient _wolfram { get; set; }

    private       bool          _loading;
    private       DiscosObject? _discosObject;
    private       string?       _id;
    private const string        _wolframAppId = "78A73E-QTTAJGPJEE";

    private async Task UpdateModel()
    {
        _loading      = true;
        _discosObject = await _client.GetSingle<DiscosObject>(_id);
        _loading      = false;
    }

    // This really ought to come from a service and be asynchronous but meh...
    private MarkupString GetImage()
    {
        try
        {
            string    wolframQuery = $"http://api.wolframalpha.com/v2/query?input=show+image+of+{_discosObject.Name}&appid={_wolframAppId}&format=image";
            string    res          = _wolfram.GetStringAsync(wolframQuery).Result;
            XDocument xml          = XDocument.Parse(res);
            XElement  img          = xml.Descendants("img").Skip(1).First();
            img.Attribute("width").Remove();
            img.Attribute("height").Remove();
            img.Add(new XAttribute("width", "100%"));
            return (MarkupString) img.ToString();
        }
        catch
        {
            return (MarkupString) "<div/>";
        }
    }

    private string GetPropLine(PropertyInfo prop) => $"{prop.Name}: {prop.GetValue(_discosObject)}";
}