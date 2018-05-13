using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IModerationRepository
    {
        Task<bool> Insert(BrandToModerate brandToModerate);
        BrandToModerate Get(Func<BrandToModerate, bool> function);
        Task<bool> SaveAsync();
        SearchResult<BrandToModerateDto> GetByParameters(BrandSearchBindingModel parameters);
    }
}
