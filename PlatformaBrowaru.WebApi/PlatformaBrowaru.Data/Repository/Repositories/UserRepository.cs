using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Exists(Func<ApplicationUser, bool> function)
        {
            return _dbContext.Users.FirstOrDefault(function) != null;
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

        public async Task<RefreshToken> GetRefreshTokenAsync(long userId)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<bool> RemoveRefreshTokenAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            return await SaveAsync();
        }

        public async Task<bool> Insert(ApplicationUser user)
        {
            await _dbContext.AddAsync(user);
            return await SaveAsync();
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            var result = _dbContext.Users.OrderBy(p => p.Id).ToList();
            return result;
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }
        
    }
}
