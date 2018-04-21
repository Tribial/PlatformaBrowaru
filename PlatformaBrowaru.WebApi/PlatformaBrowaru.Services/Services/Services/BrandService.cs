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
                BrandProduction = new BrandProduction
                {
                    AddedAt = DateTime.Now,
                    AddedBy = _userRepository.Get(u => u.Id == userId),
                    ProducedBy = _breweryRepository.Get(b => b.Id == brandBindingModel.BrandProduction.BreweryId),
                    ProducedFrom = brandBindingModel.BrandProduction.ProducedFrom,
                    ProducedTo = brandBindingModel.BrandProduction.ProducedTo,
                }
            };
            brand.BrandProduction.Brand = brand;

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
    }
}
