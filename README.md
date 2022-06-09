![GitHub Workflow Status](https://img.shields.io/github/workflow/status/hughesjs/DISOSweb-sdk/.NET?style=for-the-badge)
![GitHub top language](https://img.shields.io/github/languages/top/hughesjs/DISOSweb-sdk?style=for-the-badge)
![GitHub](https://img.shields.io/github/license/hughesjs/DISOSweb-sdk?style=for-the-badge)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/DISCOSweb-Sdk?style=for-the-badge)
![Nuget](https://img.shields.io/nuget/dt/DISCOSweb-Sdk?style=for-the-badge)

# DISCOSweb SDK

This is C# library for interfacing with the European Space Agency's DISCOSweb API.
## Installation

You can either use your IDE's package manager, or the Nuget CLI.

```bash
dotnet add package DISCOSweb-Sdk
```

Then add this to your `IServiceCollection` setup (this could be in `startup.cs` or `program.cs` depending on which style of startup you're using).

```cs
services.AddDiscosServices(configuration);
```

Your configuration should contain the following:

```json
{
    "DiscosOptions": {
        "DiscosApiKey": "YourApiKey",
        "DiscosApiUrl": "https://discosweb.esoc.esa.int/api/"
    }
}
```

## Usage

There is a demo app located in [`src/demo-app`](https://github.com/hughesjs/DISOSweb-sdk/tree/master/src/demo-app).

### Models

All models are located in the `DISCOSweb_Sdk.Models` namespace and inherit from `DiscosModelBase`.

These models are POCOs as far as is practical and match the data that comes from the API as closely as possible. However, abbreviations have been removed and names adjusted to match standard dotnet formats.

There is a slight quirk where `Latitude` and `Longitude` on most Discos models (API side) are presented as strings, which is obviously ridiculous. As such, these are mapped to `internal` fields and then recast to give us `LatitudeDegs` and `LongitudeDegs` fields that are `floats`.

Each property has a `[JsonPropertyName("name")]` that indicates how it's serialised by the API. Each `enum` has an `[EnumMember(Value = "")]` for the same purpose.

### Client

An `IDiscosClient` interface is provided for sending queries to the API. this only has two methods and should be set up by the DI extentions.

```cs
public interface IDiscosClient
{
    public Task<T> GetSingle<T>(string id, string queryString = "");
    public Task<IReadOnlyList<T>> GetMultiple<T>(string queryString = "");
}
```

`.GetSingle<T>` queries the API for a single `T` with a given ID and query string.

`.GetMultiple<T>` queries the API for multiple `T`s with a query string and returns all of them in a list.

It's worth noting that various error situations are handled internally (such as retrying on rate limit violation) and that all results are returned ready-deserialised.

### Query Building

A builder has been provided that can be used to construct query strings. This uses a fluent(ish) interface, albeit one without grammar. Most grammar rules are instead enforced through exceptions, but they should be fairly straightforward. An actual fluent interface with grammar is a planned future improvement.

```cs
public interface IDiscosQueryBuilder<TObject> where TObject : notnull
{
	public IDiscosQueryBuilder<TObject> AddFilter(FilterDefinition filterDefinition);
	public IDiscosQueryBuilder<TObject> And();
	public IDiscosQueryBuilder<TObject> Or();
	public IDiscosQueryBuilder<TObject> AddInclude(string fieldName);
	public IDiscosQueryBuilder<TObject> AddAllIncludes();
	public IDiscosQueryBuilder<TObject> AddPageSize(int numPages);
	public IDiscosQueryBuilder<TObject> AddPageNum(int  pageNum);
	public IDiscosQueryBuilder<TObject> Reset();
	public string                       Build();
}
```

The DI container will be set up to be able to provide an `IDiscosQueryBuilder<T>` for all types inheriting from `DiscosModelBase`.

#### Filters

Filters are the most complicated of the parameters that can be built since they can be infinitely combined.

A filter definition is represented by a generic `FilterDefinition<TObject, TParam>` where `TObject` is the type of the object being fetched from the API (e.g. `Propellant`), and `TParam` is the type of the parameter you'll be querying for (e.g. if you're looking for `.Height`, `TParam` is `float`).

These filters can then be added to the builder using `.AddFilter(myFilter)`.

To combine filters, the `.And()` and `.Or()` methods are used. This builds up an expression tree which is then parsed to create the final query string. The tree must be complete when `.Build()` is called or an exception will be thrown.

For example, given two `FilterDefinition`s `f1` and `f2`, we could query for `f1` and `f2` by doing:

```cs
builder.AddFilter(f1).And().AddFilter(f2);
```

#### Relationships

A JSON API (such as DISCOS) provides links to related objects. For instance, a `LaunchVehicle` has an `Operator`. By default, these won't be fetched by the client. In order to include them, they need to be added to the query through one of the two provided methods.

```cs
builder.AddInclude(fieldName);
builder.AddAllIncludes();
```

`.AddInclude(fieldName)` will include a single linked property (use the C# fieldname, not the discos fieldname - this is what `nameof` is good for). To include different combinations, daisy chain these calls. 

`.AddAllIncludes()` will add every possible include for a given type.

It should be noted that the JSON API does not support recursive fetching. So you can only fetch one layer of related objecs.

There are plans to add lazy loading of these properties on-fetch but that has not been implemented yet.

#### Paging

The DISCOS API returns data in pages. These can be manipulated using the `.AddPageNum(numPages)` and `.AddPageSize(size)` methods. They do exactly what you'd expect.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

