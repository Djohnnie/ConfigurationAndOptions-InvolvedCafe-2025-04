using CustomConfigurationProvider.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CustomConfigurationProvider.Data;

public class ConfigurationDbContext : DbContext
{
    public DbSet<ConfigurationSetting> Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid}");
    }
}