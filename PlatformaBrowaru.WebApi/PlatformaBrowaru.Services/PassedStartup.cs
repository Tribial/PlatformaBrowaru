using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformaBrowaru.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Data.Repository.Repositories;

namespace PlatformaBrowaru.Services
{
    public static class PassedStartup
    {
        public static void Config(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();

            var configure = new StartUpConfig(configuration);
            configure.PartOfConfigureServices(services);
        }
    }
}
