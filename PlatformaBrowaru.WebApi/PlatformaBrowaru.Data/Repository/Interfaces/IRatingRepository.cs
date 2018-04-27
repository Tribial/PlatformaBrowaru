using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IRatingRepository
    {
        Rating Get(Func<Rating, bool> function);
        Task<bool> SaveAsync();
        Task<bool> DeleteAsync(Rating rating);
    }
}
