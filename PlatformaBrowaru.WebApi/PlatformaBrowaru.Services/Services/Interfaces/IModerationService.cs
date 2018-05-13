using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Interfaces
{
    public interface IModerationService
    {
        Task<ResponseDto<BaseModelDto>> AddForModeration(long userId, AddToModerationBindingModel addToModeration);
        ResponseDto<SearchResult<BrandToModerateDto>> GetBrandsToModerate( BrandSearchBindingModel parameters);
    }
}
