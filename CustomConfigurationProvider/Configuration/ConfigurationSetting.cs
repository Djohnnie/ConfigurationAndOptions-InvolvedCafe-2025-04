﻿namespace CustomConfigurationProvider.Configuration;

public class ConfigurationSetting
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}