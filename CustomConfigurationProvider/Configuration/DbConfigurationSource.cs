namespace CustomConfigurationProvider.Configuration;

public class DbConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new DbConfigurationProvider();
}