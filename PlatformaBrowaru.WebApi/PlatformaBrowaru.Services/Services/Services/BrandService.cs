using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IEnumerationRepository _enumerationRepository;
        private readonly IKindRepository _kindRepository;
        private readonly IBreweryRepository _breweryRepository;

        public BrandService(IUserRepository userRepository, IBrandRepository brandRepository, IEnumerationRepository enumerationRepository, IKindRepository kindRepository, IBreweryRepository breweryRepository)
        {
            _userRepository = userRepository;
            _brandRepository = brandRepository;
            _enumerationRepository = enumerationRepository;
            _kindRepository = kindRepository;
            _breweryRepository = breweryRepository;
        }

        public async Task<ResponseDto<BaseModelDto>> AddBeerBrandAsync(long userId, BrandBindingModel brandBindingModel)
        {
            var result = new ResponseDto<BaseModelDto>();

            Brand brand = new Brand
            {
                Name = brandBindingModel.Name,
                Description = brandBindingModel.Description,
                Ingredients = brandBindingModel.Ingredients,
                Color = brandBindingModel.Color,
                AlcoholAmountPercent = brandBindingModel.AlcoholAmountPercent,
                ExtractPercent = brandBindingModel.ExtractPercent,
                HopIntensity = brandBindingModel.HopIntensity ?? -1,
                TasteFullness = brandBindingModel.TasteFullness ?? -1,
                Sweetness = brandBindingModel.Sweetness ?? -1,
                Kind = _kindRepository.Get(k => k.Id == brandBindingModel.KindId), 
                BrandSeasons = new List<BrandSeason>(),
                BrandBrewingMethods = new List<BrandBrewingMethod>(),
                BrandWrappings = new List<BrandWrapping>(),
                CreationDate = brandBindingModel.CreationDate,
                AddedBy = _userRepository.Get(u => u.Id == userId),
                AddedAt = DateTime.Now,
                IsAccepted = false,
                IsPasteurized = brandBindingModel.IsPasteurized,
                IsFiltered = brandBindingModel.IsFiltered,
                Ratings = new List<Rating>(),
                Reviews = new List<Review>(),
            };

            var insertResult = await _brandRepository.InsertAsync(brand);

            if (!insertResult)
            {
                result.Errors.Add("Coś poszło nie tak! Spróbuj powonie później");
                return result;
            }

            brandBindingModel.SeasonIds.ForEach(s =>
                brand.BrandSeasons.Add(
                    new BrandSeason
                    {
                        Brand = brand,
                        BrandId = brand.Id,
                        Season = _enumerationRepository.GetSeason(er => er.Id == s),
                        SeasonId = s
                    })
                );

            brandBindingModel.BrewingMethodIds.ForEach(b =>
                brand.BrandBrewingMethods.Add(
                    new BrandBrewingMethod
                    {
                        Brand = brand,
                        BrandId = brand.Id,
                        BrewingMethod = _enumerationRepository.GetBrewingMethod(er => er.Id == b),
                        BrewingMethodId = b
                    })
                );


            brandBindingModel.WrappingIds.ForEach(w =>
                brand.BrandWrappings.Add(
                    new BrandWrapping
                    {
                        Brand = brand,
                        BrandId = brand.Id,
                        Wrapping = _enumerationRepository.GetWrapping(er => er.Id == w),
                        WrappingId = w
                    })
                );


            var updateResult = await _brandRepository.UpdateAsync(brand);

            if (!updateResult)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }

            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> EditBeerBrandAsync(long userId, long brandId, EditBrandBindingModel beerBrand)
        {
            var result = new ResponseDto<BaseModelDto>();

            var brand = _brandRepository.Get(u => u.Id == brandId);
            brand.Name = beerBrand.Name;
            brand.Description = beerBrand.Description;
            brand.Ingredients = beerBrand.Ingredients;
            brand.Color = beerBrand.Color;
            brand.AlcoholAmountPercent = beerBrand.AlcoholAmountPercent;
            brand.ExtractPercent = beerBrand.ExtractPercent;
            brand.HopIntensity = beerBrand.HopIntensity ?? -1;
            brand.TasteFullness = beerBrand.TasteFullness ?? -1;
            brand.Sweetness = beerBrand.Sweetness ?? -1;
            brand.Kind = _kindRepository.Get(k => k.Id == beerBrand.KindId);
            brand.IsPasteurized = beerBrand.IsPasteurized;
            brand.IsFiltered = beerBrand.IsFiltered;
            brand.BrandProduction = new BrandProduction
            {
                EditedAt = DateTime.Now,
                EditedBy = _userRepository.Get(u => u.Id == userId),
                ProducedBy = _breweryRepository.Get(b => b.Id == beerBrand.BrandProduction.BreweryId),
                ProducedFrom = beerBrand.BrandProduction.ProducedFrom,
                ProducedTo = beerBrand.BrandProduction.ProducedTo,
            };
            brand.CreationDate = beerBrand.CreationDate;
            brand.EditedAt = DateTime.Now;
            brand.EditedBy = _userRepository.Get(u => u.Id == userId);

            beerBrand.SeasonIds.ForEach(z =>
                brand.BrandSeasons.Add(
                    new BrandSeason
                    {
                        Brand = brand,
                        BrandId = brand.Id,
                        Season = _enumerationRepository.GetSeason(x => x.Id == z),
                        SeasonId = z
                    })
                );

            beerBrand.FermentationTypeIds.ForEach(s =>
                brand.BrandFermentationTypes.Add(
                    new BrandFermentationType
                    {
                        Brand = brand,
                        BrandId = brand.Id,
                        FermentationType = _enumerationRepository.GetFermentation(x => x.Id == s),
                        FermentationTypeId = s
                    })
                );

            beerBrand.BrewingMethodIds.ForEach(c =>
                brand.BrandBrewingMethods.Add(
                    new BrandBrewingMethod
                    {
                        Brand = brand,
                        BrandId = brand.Id,
                        BrewingMethod = _enumerationRepository.GetBrewingMethod(x => x.Id == c),
                        BrewingMethodId = c
                    })
                );


            beerBrand.WrappingIds.ForEach(v =>
                brand.BrandWrappings.Add(
                    new BrandWrapping
                    {
                        Brand = brand,
                        BrandId = brand.Id,
                        Wrapping = _enumerationRepository.GetWrapping(x => x.Id == v),
                        WrappingId = v
                    })
                );


            var updateResult = await _brandRepository.UpdateAsync(brand);

            if (!updateResult)
            {
                result.Errors.Add("Wystąpił nieoczekiwany błąd, spróbuj ponownie później");
            }

            return result;

        }

        public ResponseDto<SearchResult<BrandForSearchDto>> GetBrands(long userId, BrandSearchBindingModel parametes)
        {
            var result = new ResponseDto<SearchResult<BrandForSearchDto>>();

            var brands = _brandRepository.GetByParameters(userId, parametes);

            if (brands.TotalPageCount == 0)
            {
                result.Errors.Add("Nie znaleziono projektów");
                return result;
            }

            if (parametes.PageNumber > brands.TotalPageCount)
            {
                result.Errors.Add($"Strona {parametes.PageNumber} wykracza poza limit {brands.TotalPageCount}");
                return result;
            }

            result.Object = brands;
            return result;
        }
    }
}
