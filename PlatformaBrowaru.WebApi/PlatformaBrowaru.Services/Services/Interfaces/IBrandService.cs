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
        ResponseDto<GetBeerBrandDto> GetBeerBrand(long beerBrandId, long userId);
        Task<ResponseDto<BaseModelDto>> DeleteBeerBrandAsync(long beerBrandId, long userId, DeleteBeerBrandBindingModel deleteBrandModel);
        Task<ResponseDto<BaseModelDto>> AddOrEditRatingAsync(long beerBrandId, long userId, AddRatingBindingModel addRatingModel);
        Task<ResponseDto<BaseModelDto>> DeleteRatingAsync(long beerBrandId, long userId);
        ResponseDto<SearchResult<BrandForSearchDto>> GetBrands(long userId, BrandSearchBindingModel parametes);
    }
}
