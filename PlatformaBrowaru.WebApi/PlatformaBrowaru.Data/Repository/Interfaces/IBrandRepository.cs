using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IBrandRepository
    {
        Task<bool> InsertAsync(Brand brand);
        Task<bool> SaveAsync();
        bool Save();
        Task<bool> UpdateAsync(Brand brand);
    }
}
