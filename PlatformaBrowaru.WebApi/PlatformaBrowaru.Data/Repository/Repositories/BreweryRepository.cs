using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class BreweryRepository : IBreweryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BreweryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Brewery Get(Func<Brewery, bool> function)
        {
            return _dbContext.Breweries.FirstOrDefault(function);
        }
    }
}
