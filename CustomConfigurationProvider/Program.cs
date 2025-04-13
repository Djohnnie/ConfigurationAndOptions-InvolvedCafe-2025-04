using CustomConfigurationProvider.Configuration;
using Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddDbConfiguration();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config", (IConfiguration config) =>
{
    var configValues = config.AsEnumerable()
        .Where(kvp => kvp.Key.StartsWith("MySettings:"))
        .OrderBy(kvp => kvp.Key)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    return Results.Text(HtmlHelper.ToHtmlTable(configValues), contentType: "text/html");
});

app.Run();