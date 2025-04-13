using CustomConfigurationProvider.Data;

namespace CustomConfigurationProvider.Configuration;

public class DbConfigurationProvider : ConfigurationProvider
{
    public override void Load()
    {
        using var dbContext = new ConfigurationDbContext();

        dbContext.Database.EnsureCreated();

        if (!dbContext.Settings.Any())
        {
            dbContext.Settings.Add(new ConfigurationSetting
            {
                Key = "MySettings:Setting2",
                Value = "DbValue2"
            });
            dbContext.Settings.Add(new ConfigurationSetting
            {
                Key = "MySettings:Setting3",
                Value = "DbValue3"
            });
            dbContext.SaveChanges();
        }

        Data = dbContext.Settings.ToList().ToDictionary(x => x.Key, x => x.Value) ?? new();
    }
}