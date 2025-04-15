using Helpers;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ConfigValues>()
                .BindConfiguration("MySettings")
                .ValidateDataAnnotations()
                .ValidateOnStart();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config", (HttpContext httpContext, IOptionsMonitor<ConfigValues> options) =>
{
    var headers = httpContext.Request.Headers;

    if (headers.ContainsKey("Accept") && headers["Accept"].Contains("application/json"))
    {
        return Results.Ok(options.CurrentValue);
    }
    else
    {
        return Results.Text(HtmlHelper.ToHtmlTable(options.CurrentValue), contentType: "text/html");
    }
});

app.Run();

class ConfigValues
{
    public int Number { get; set; }

    [Required(AllowEmptyStrings = false), MinLength(20), MaxLength(200)]
    public string Text { get; set; } = string.Empty;

    public bool Flag { get; set; }
}