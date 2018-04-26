using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Interfaces
{
    public interface IBrandService
    {
        Task<ResponseDto<BaseModelDto>> AddBeerBrandAsync(long userId, BrandBindingModel brandBindingModel);
        Task<ResponseDto<BaseModelDto>> EditBeerBrandAsync(long userId, long brandId, EditBrandBindingModel beerBrand);
        ResponseDto<SearchResult<BrandForSearchDto>> GetBrands(long userId, BrandSearchBindingModel parametes);
    }
}
