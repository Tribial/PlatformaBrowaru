using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IModerationRepository
    {
        Task<bool> Insert(BrandToModerate brandToModerate);
        BrandToModerate Get(Func<BrandToModerate, bool> function);
        Task<bool> SaveAsync();
    }
}
