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
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RatingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Rating Get(Func<Rating, bool> function)
        {
            var result = _dbContext.Ratings.FirstOrDefault(function);
            return result;
        }
        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(Rating rating)
        {
            _dbContext.Ratings.Remove(rating);
            return await SaveAsync();
        }
    }
}
