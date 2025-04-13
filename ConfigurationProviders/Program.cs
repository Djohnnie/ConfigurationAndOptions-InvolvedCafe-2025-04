using Helpers;

const string ConnectionString = "xxx";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureAppConfiguration(ConnectionString);

builder.Configuration.AddJsonFile("CustomSettings.json");

builder.Configuration.AddXmlFile("CustomSettings.xml");

builder.Configuration.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
    { new("MySettings:Setting7", "MemoryValue7") });


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