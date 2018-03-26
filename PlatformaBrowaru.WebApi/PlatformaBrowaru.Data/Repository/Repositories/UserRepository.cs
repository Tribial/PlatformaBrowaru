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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApplicationUser Get(Func<ApplicationUser, bool> function)
        {
            var result = _dbContext.Users.FirstOrDefault(function);
            return result;
        }

        public async Task<bool> InsertRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            return await SaveAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(long userIdAsString)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userIdAsString);
        }

        public async Task<bool> RemoveRefreshTokenAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            return await SaveAsync();
        }

        private async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        private bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
