using Helpers;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddEnvironmentVariables();
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/config", (HttpContext httpContext, IConfiguration config) =>
{
    var headers = httpContext.Request.Headers;
    var configValues = config.AsEnumerable()
        .Where(kvp => kvp.Key.StartsWith("MySettings:"))
        .OrderBy(kvp => kvp.Key)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    if (headers.ContainsKey("Accept") && headers["Accept"].Contains("application/json"))
    {
        return Results.Ok(configValues);
    }
    else
    {
        return Results.Text(HtmlHelper.ToHtmlTable(configValues), contentType: "text/html");
    }
});

app.Run();