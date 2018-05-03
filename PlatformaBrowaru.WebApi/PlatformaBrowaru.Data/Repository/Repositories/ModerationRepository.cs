using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class ModerationRepository : IModerationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ModerationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Insert(BrandToModerate brandToModerate)
        {
            await _dbContext.BrandsToModerate.AddAsync(brandToModerate);
            return await SaveAsync();
        }

        public BrandToModerate Get(Func<BrandToModerate, bool> function)
        {
            return _dbContext.BrandsToModerate.FirstOrDefault(function);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
