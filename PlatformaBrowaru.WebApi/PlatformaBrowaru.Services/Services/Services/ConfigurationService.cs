using System;
using Microsoft.Extensions.Configuration;
using PlatformaBrowaru.Services.Services.Interfaces;

namespace PlatformaBrowaru.Services.Services.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetValue(string key)
        {
            return _configuration[key];
        }
    }
}