using Helpers;

const string ConnectionString = "xxx";

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureAppConfiguration(o =>
{
    o.Connect(ConnectionString);
    o.ConfigureRefresh(ro =>
    {
        ro.Register("Sentinel", refreshAll: true);
    });
});

builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAzureAppConfiguration();


app.MapGet("/config", (IConfiguration config) =>
{
    var configValues = config.AsEnumerable()
        .Where(kvp => kvp.Key.StartsWith("MySettings:"))
        .OrderBy(kvp => kvp.Key)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    return Results.Text(HtmlHelper.ToHtmlTable(configValues), contentType: "text/html");
});

app.Run();