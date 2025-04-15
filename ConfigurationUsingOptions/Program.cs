using Helpers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ConfigValues>().BindConfiguration("MySettings");

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config", (HttpContext httpContext, IOptions<ConfigValues> options) =>
{
    var headers = httpContext.Request.Headers;

    if (headers.ContainsKey("Accept") && headers["Accept"].Contains("application/json"))
    {
        return Results.Ok(options.Value);
    }
    else
    {
        return Results.Text(HtmlHelper.ToHtmlTable(options.Value), contentType: "text/html");
    }
});

app.Run();

class ConfigValues
{
    public int Number { get; set; }
    public string? Text { get; set; }
    public bool Flag { get; set; }
}