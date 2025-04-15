using Helpers;
using Microsoft.AspNetCore.Http;
using System.Reflection.PortableExecutable;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config", (HttpContext httpContext, int type, IConfiguration config) =>
{
    var headers = httpContext.Request.Headers;
    var configValues = type switch
    {
        1 => GetUsingBasicIndexers(type, config),
        2 => GetUsingGetValue(type, config),
        3 => GetUsingGetSectionAndBind(type, config),
        _ => throw new InvalidOperationException()
    };

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



static ConfigValues GetUsingBasicIndexers(int type, IConfiguration config)
{
    return new ConfigValues
    {
        Number = Convert.ToInt32(config["MySettings:Number"]),
        Text = config["MySettings:Text"],
        Flag = Convert.ToBoolean(config["MySettings:Flag"]),
        Type = type
    };
}

static ConfigValues GetUsingGetValue(int type, IConfiguration config)
{
    return new ConfigValues
    {
        Number = config.GetValue<int>("MySettings:Number"),
        Text = config.GetValue<string>("MySettings:Text"),
        Flag = config.GetValue<bool>("MySettings:Flag"),
        Type = type
    };
}

static ConfigValues GetUsingGetSectionAndBind(int type, IConfiguration config)
{
    var configValues = new ConfigValues { Type = type };
    config.GetSection("MySettings").Bind(configValues);

    return configValues;
}

class ConfigValues
{
    public int Number { get; set; }
    public string? Text { get; set; }
    public bool Flag { get; set; }
    public int Type { get; set; }
}