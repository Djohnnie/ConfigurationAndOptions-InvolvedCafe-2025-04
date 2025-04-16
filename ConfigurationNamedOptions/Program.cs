using Helpers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ConfigValues>("A").BindConfiguration("MySettings:A");
builder.Services.AddOptions<ConfigValues>("B").BindConfiguration("MySettings:B");

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config/{section}", (HttpContext httpContext, string section, IOptionsSnapshot<ConfigValues> options) =>
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
}