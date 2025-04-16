using Helpers;

const string ConnectionString = "Endpoint=https://config-involvedcafe2025.azconfig.io;Id=JnWH;Secret=EdA260QwmfxmP3vKP6IGoXKmtTeZZZnNMF7w4st2X8Gv60Lpjxd9JQQJ99BDACi5YpzXpOL0AAACAZAC3GWN";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureAppConfiguration(ConnectionString);

builder.Configuration.AddJsonFile("CustomSettings.json");

builder.Configuration.AddXmlFile("CustomSettings.xml");

builder.Configuration.AddIniFile("CustomSettings.ini");

builder.Configuration.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
    { new("MySettings:Setting7", "MemoryValue7") });


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