using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class EnumerationRepository : IEnumerationRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public EnumerationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Season GetSeason(Func<Season, bool> function)
        {
            return _dbContext.Seasons.FirstOrDefault(function);
        }

        public BrewingMethod GetBrewingMethod(Func<BrewingMethod, bool> function)
        {
            return _dbContext.BrewingMethods.FirstOrDefault(function);
        }

        public Wrapping GetWrapping(Func<Wrapping, bool> function)
        {
            return _dbContext.Wrappings.FirstOrDefault(function);
        }

        public FermentationType GetFermentation (Func<FermentationType, bool> function)
        {
            return _dbContext.FermentationTypes.FirstOrDefault(function);
        }
    }
}
