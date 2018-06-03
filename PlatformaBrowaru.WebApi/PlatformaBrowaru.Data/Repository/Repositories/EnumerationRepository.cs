using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public Season GetSeasonWithoutTracking(Func<Season, bool> function)
        {
            return _dbContext.Seasons.AsNoTracking().FirstOrDefault(function);
        }

        public BrewingMethod GetBrewingMethodWithoutTracking(Func<BrewingMethod, bool> function)
        {
            return _dbContext.BrewingMethods.AsNoTracking().FirstOrDefault(function);
        }

        public Wrapping GetWrappingWithoutTracking(Func<Wrapping, bool> function)
        {
            return _dbContext.Wrappings.AsNoTracking().FirstOrDefault(function);
        }

        public FermentationType GetFermentationWithoutTracking(Func<FermentationType, bool> function)
        {
            return _dbContext.FermentationTypes.AsNoTracking().FirstOrDefault(function);
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

        public FermentationType GetFermentation(Func<FermentationType, bool> function)
        {
            return _dbContext.FermentationTypes.FirstOrDefault(function);
        }

        public async Task<bool> ClearEnumerationsForBrand(long brandId)
        {
            var brand = _dbContext.Brands
                .Include(b => b.BrandWrappings)
                .Include(b => b.BrandSeasons)
                .Include(b => b.BrandFermentationTypes)
                .Include(b => b.BrandBrewingMethods)
                .FirstOrDefault(b => b.Id == brandId);

            if (brand == null)
            {
                return false;
            }

            brand.BrandWrappings.ForEach(b => _dbContext.Remove(b));
            brand.BrandSeasons.ForEach(b => _dbContext.Remove(b));
            brand.BrandFermentationTypes.ForEach(b => _dbContext.Remove(b));
            brand.BrandBrewingMethods.ForEach(b => _dbContext.Remove(b));

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
