namespace CustomConfigurationProvider.Configuration;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddDbConfiguration(this ConfigurationManager manager)
    {
        IConfigurationBuilder configurationBuilder = manager;
        configurationBuilder.Add(new DbConfigurationSource());

        return manager;
    }
}