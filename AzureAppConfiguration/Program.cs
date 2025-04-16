using Helpers;

const string ConnectionString = "Endpoint=https://config-involvedcafe2025.azconfig.io;Id=JnWH;Secret=EdA260QwmfxmP3vKP6IGoXKmtTeZZZnNMF7w4st2X8Gv60Lpjxd9JQQJ99BDACi5YpzXpOL0AAACAZAC3GWN";

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