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
        ResponseDto<SearchResult<BrandForSearchDto>> GetBrands(long userId, SearchBindingModel parametes);
        Task<ResponseDto<BaseModelDto>> AddReviewAsync(long beerBrandId, long userId, AddReviewBindingModel addReviewModel);
        Task<ResponseDto<BaseModelDto>> EditReviewAsync(long beerBrandId, long userId, EditReviewBindingModel editReviewModel, string userRole, long reviewId);
        Task<ResponseDto<BaseModelDto>> DeleteReviewAsync(long beerBrandId, long userId, string userRole, long reviewId);
    }
}
