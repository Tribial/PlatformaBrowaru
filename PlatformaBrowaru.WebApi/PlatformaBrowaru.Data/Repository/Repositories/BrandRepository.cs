using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> InsertAsync(Brand brand)
        {
            await _dbContext.Brands.AddAsync(brand);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(Brand brand)
        {
            _dbContext.Brands.Update(brand);
            return await SaveAsync();
        }
    }
}
