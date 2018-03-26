using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformaBrowaru.Data.Data;

namespace PlatformaBrowaru.Data
{
    public class StartUpConfig
    {
        public IConfiguration Configuration;
        public static string Server;
        public StartUpConfig(IConfiguration configuration)
        {
            Configuration = configuration;
            Server = Configuration.GetConnectionString("DefaultConnection");
        }

        public void PartOfConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        }
    }
}
