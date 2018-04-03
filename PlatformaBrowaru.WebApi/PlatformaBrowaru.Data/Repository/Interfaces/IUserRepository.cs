using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
        bool Exists(Func<ApplicationUser, bool> function);
        ApplicationUser Get(Func<ApplicationUser, bool> function);
        Task<bool> InsertRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(long userId);
        Task<bool> RemoveRefreshTokenAsync(RefreshToken refreshToken);
    }
}
