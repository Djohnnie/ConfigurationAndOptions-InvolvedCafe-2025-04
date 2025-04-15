using Helpers;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddOptions<ConfigValues>().BindConfiguration("MySettings");

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, ConfigValuesContext.Default);
});

var app = builder.Build();

app.MapGet("/config", (IOptions<ConfigValues> options) =>
{
    return Results.Ok(options.Value);
});

app.Run();

class ConfigValues
{
    public int Number { get; set; }
    public string? Text { get; set; }
    public bool Flag { get; set; }
}

[JsonSerializable(typeof(ConfigValues))]
partial class ConfigValuesContext : JsonSerializerContext { }