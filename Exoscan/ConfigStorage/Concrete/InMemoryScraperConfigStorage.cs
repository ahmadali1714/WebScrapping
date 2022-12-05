﻿using Exoscan.ConfigStorage.Abstract;

namespace Exoscan.ConfigStorage.Concrete;

public class InMemoryScraperConfigStorage: IScraperConfigStorage
{
    private ScraperConfig _config;

    public Task CreateConfigAsync(ScraperConfig config)
    {
        _config = config;
        return Task.CompletedTask;
    }

    public Task<ScraperConfig> GetConfigAsync()
    {
        return Task.FromResult(_config);
    }
}