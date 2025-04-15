using Helpers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ConfigValues>().BindConfiguration("MySettings");

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config/options", (HttpContext httpContext, IOptions<ConfigValues> options) =>
{
    return Response(httpContext, options.Value);
});

app.MapGet("/config/monitor", (HttpContext httpContext, IOptionsMonitor<ConfigValues> options) =>
{
    return Response(httpContext, options.CurrentValue);
});

app.MapGet("/config/snapshot", (HttpContext httpContext, IOptionsSnapshot<ConfigValues> options) =>
{
    return Response(httpContext, options.Value);
});

app.Run();

static IResult Response(HttpContext httpContext, ConfigValues values)
{
    var headers = httpContext.Request.Headers;

    if (headers.ContainsKey("Accept") && headers["Accept"].Contains("application/json"))
    {
        return Results.Ok(values);
    }
    else
    {
        return Results.Text(HtmlHelper.ToHtmlTable(values), contentType: "text/html");
    }
}

class ConfigValues
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Number { get; set; }
    public string? Text { get; set; }
    public bool Flag { get; set; }
}