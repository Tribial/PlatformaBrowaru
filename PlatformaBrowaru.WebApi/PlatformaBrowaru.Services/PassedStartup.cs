using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformaBrowaru.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Data.Repository.Repositories;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.BindingModels;

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

        public static void AutoMapperConfiguration()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserProfileBindingModel, ApplicationUser>();
            });
        }
    }
}
