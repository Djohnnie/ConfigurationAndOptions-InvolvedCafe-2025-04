using Helpers;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/config", (int type, IConfiguration config) =>
{
    var configValues = type switch
    {
        1 => GetUsingBasicIndexers(config),
        2 => GetUsingGetValue(config),
        3 => GetUsingGetSectionAndBind(config),
        _ => throw new InvalidOperationException()
    };

    return Results.Text(HtmlHelper.ToHtmlTable(configValues), contentType: "text/html");
});

app.Run();



static ConfigValues GetUsingBasicIndexers(IConfiguration config)
{
    return new ConfigValues
    {
        Number = Convert.ToInt32(config["MySettings:Number"]),
        Text = config["MySettings:Text"],
        Flag = Convert.ToBoolean(config["MySettings:Flag"])
    };
}

static ConfigValues GetUsingGetValue(IConfiguration config)
{
    return new ConfigValues
    {
        Number = config.GetValue<int>("MySettings:Number"),
        Text = config.GetValue<string>("MySettings:Text"),
        Flag = config.GetValue<bool>("MySettings:Flag")
    };
}

static ConfigValues GetUsingGetSectionAndBind(IConfiguration config)
{
    var configValues = new ConfigValues();
    config.GetSection("MySettings").Bind(configValues);

    return configValues;
}

class ConfigValues
{
    public int Number { get; set; }
    public string? Text { get; set; }
    public bool Flag { get; set; }
}