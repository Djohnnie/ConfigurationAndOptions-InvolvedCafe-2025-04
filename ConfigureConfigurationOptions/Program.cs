using Helpers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ConfigValues>("A").BindConfiguration("MySettings:A");
builder.Services.AddOptions<ConfigValues>("B").BindConfiguration("MySettings:B")
    .Configure(c =>
    {
        c.Number = 42;
        c.Events.Add($"OptionsBuilder.Configure called with {nameof(c.Number)}={c.Number}");
    })
    .PostConfigure(c =>
    {
        c.Number = 43;
        c.Events.Add($"OptionsBuilder.PostConfigure called with {nameof(c.Number)}={c.Number}");
    });
builder.Services.AddOptions<ConfigValues>("C").BindConfiguration("MySettings:C");
builder.Services.PostConfigureAll<ConfigValues>(c =>
{
    c.Number = -1;
    c.Events.Add($"IServiceCollection.PostConfigureAll called with {nameof(c.Number)}={c.Number}");
});
builder.Services.PostConfigure<ConfigValues>("C", c =>
{
    c.Number = 1234;
    c.Events.Add($"IServiceCollection.PostConfigure called with {nameof(c.Number)}={c.Number}");
});
builder.Services.ConfigureAll<ConfigValues>(c =>
{
    c.Number = 666;
    c.Events.Add($"IServiceCollection.ConfigureAll called with {nameof(c.Number)}={c.Number}");
});

builder.Services.Configure<ConfigValues>("B", c =>
{
    c.Number = 1024;
    c.Events.Add($"IServiceCollection.Configure[B] called with {nameof(c.Number)}={c.Number}");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config/{section}", (HttpContext httpContext, string section, IOptionsMonitor<ConfigValues> options) =>
{
    var headers = httpContext.Request.Headers;

    var namedOptions = options.Get(section);

    if (headers.ContainsKey("Accept") && headers["Accept"].Contains("application/json"))
    {
        return Results.Ok(namedOptions);
    }
    else
    {
        return Results.Text(HtmlHelper.ToHtmlTable(namedOptions), contentType: "text/html");
    }
});

app.Run();

class ConfigValues
{
    public int Number { get; set; }
    public string? Text { get; set; }
    public bool Flag { get; set; }
    public List<string> Events { get; set; } = [];

    public ConfigValues()
    {
        Events.Add("ConfigValues constructor called");
    }
}