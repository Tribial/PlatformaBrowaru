using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private readonly IRatingRepository _ratingRepository;
        private readonly IModerationRepository _moderationRepository;

        public BrandService(IUserRepository userRepository, IBrandRepository brandRepository,
            IEnumerationRepository enumerationRepository, IKindRepository kindRepository,
            IBreweryRepository breweryRepository, IRatingRepository ratingRepository, IModerationRepository moderationRepository)
        {
            _userRepository = userRepository;
            _brandRepository = brandRepository;
            _enumerationRepository = enumerationRepository;
            _kindRepository = kindRepository;
            _breweryRepository = breweryRepository;
            _ratingRepository = ratingRepository;
            _moderationRepository = moderationRepository;
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
                BrandFermentationTypes = new List<BrandFermentationType>(),
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
            if (_brandRepository.Get(x => x.Name == brand.Name) != null)
            {
                result.Errors.Add("Gatunek o podanej nazwie już istnieje.");
                return result;
            }
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

            brandBindingModel.FermentationTypeIds.ForEach(s =>
                brand.BrandFermentationTypes.Add(
                    new BrandFermentationType
                    {
                        Brand = brand,
                        BrandId = brand.Id,
                        FermentationType = _enumerationRepository.GetFermentation(x => x.Id == s),
                        FermentationTypeId = s
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

        public async Task<ResponseDto<BaseModelDto>> EditBeerBrandAsync(long userId, long brandId,
            EditBrandBindingModel beerBrand)
        {
            var result = new ResponseDto<BaseModelDto>();
            var user = _userRepository.Get(x => x.Id == userId);
            if (user.Role == "User")
            {
                result.Errors.Add("Nie masz uprawnień do wykonania tej operacji.");
                return result;
            }

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
            if (user.Role == "Moderator" || user.Role == "Administrator")
            {
                brand.IsAccepted = true;
                var brandToModerate = _moderationRepository.Get(m => m.Id == brand.Id);
                if (brandToModerate != null)
                {
                    var deletionResult = await _moderationRepository.Remove(brandToModerate);
                    if (!deletionResult)
                    {
                        result.Errors.Add("Wystąpił nieoczekiwany błąd, spróbuj ponownie później");
                    }
                }
            }

            var updateResult = await _brandRepository.UpdateAsync(brand);

            if (!updateResult)
            {
                result.Errors.Add("Wystąpił nieoczekiwany błąd, spróbuj ponownie później");
                return result;
            }

            return result;

        }

        public ResponseDto<GetBeerBrandDto> GetBeerBrand(long beerBrandId, long userId)
        {
            var brand = _brandRepository.Get(x => x.Id == beerBrandId);

            var result = new ResponseDto<GetBeerBrandDto>
            {
                Errors = new List<string>(),
                Object = new GetBeerBrandDto()
            };

            var wrappings = new List<string>();
            brand.BrandWrappings.ForEach(brandWrapping =>
            {
                if (brandWrapping.BrandId == brand.Id)
                {
                    wrappings.Add(_enumerationRepository
                        .GetWrapping(wrapping => wrapping.Id == brandWrapping.WrappingId).Name);
                }
            });
            var seasons = new List<string>();
            brand.BrandSeasons.ForEach(brandSeason =>
            {
                if (brandSeason.BrandId == brand.Id)
                {
                    seasons.Add(_enumerationRepository.GetSeason(season => season.Id == brandSeason.SeasonId).Name);
                }
            });
            var brewingMethod = "";
            brand.BrandBrewingMethods.ForEach(brandBrewingMethod =>
            {
                if (brandBrewingMethod.BrandId == brand.Id)
                {
                    brewingMethod = _enumerationRepository
                        .GetBrewingMethod(x => x.Id == brandBrewingMethod.BrewingMethodId).Method;
                }
            });
            var fermentationType = "";
            brand.BrandFermentationTypes.ForEach(brandFermentationType =>
            {
                if (brandFermentationType.BrandId == brand.Id)
                {
                    fermentationType = _enumerationRepository
                        .GetFermentation(x => x.Id == brandFermentationType.FermentationTypeId).Type;
                }
            });
            result.Object.Name = brand.Name;
            result.Object.Description = brand.Description;
            result.Object.Ingredients = brand.Ingredients;
            result.Object.Kind = brand.Kind.Name;
            result.Object.Color = brand.Color;
            result.Object.AlcoholAmountPercent = brand.AlcoholAmountPercent;
            result.Object.ExtractPercent = brand.ExtractPercent;
            if (brand.Ratings.Count != 0)
            {
                result.Object.GeneralRate = brand.Ratings.Sum(rating => rating.Rate) / brand.Ratings.Count;
                
            }
            else
            {
                result.Object.GeneralRate = 0;
            }
            result.Object.YourRate = brand.Ratings.Find(r => r.Author.Id == userId)?.Rate;
            result.Object.BrandWrappings = wrappings;
            result.Object.HopIntensity = brand.HopIntensity;
            result.Object.TasteFullness = brand.TasteFullness;
            result.Object.Sweetness = brand.Sweetness;
            result.Object.BrandSeasons = seasons;
            result.Object.CreationDate = brand.CreationDate;
            result.Object.BrandBrewingMethod = brewingMethod;
            result.Object.BrandFermentationType = fermentationType;
            result.Object.IsPasteurized = brand.IsPasteurized;
            result.Object.IsFiltered = brand.IsFiltered;

            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> DeleteBeerBrandAsync(long beerBrandId, long userId, DeleteBeerBrandBindingModel deleteBrandModel)
        {
            var brand = _brandRepository.Get(x => x.Id == beerBrandId);

            var result = new ResponseDto<BaseModelDto>();
            brand.DeletedBy = _userRepository.Get(u => u.Id == userId);
            brand.DeletionReason = deleteBrandModel.DeletionReason;

            var updateResult = await _brandRepository.UpdateAsync(brand);
            if (!updateResult)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }

            return result;
        }

        public ResponseDto<SearchResult<BrandForSearchDto>> GetBrands(long userId, SearchBindingModel parametes)
        {
            var result = new ResponseDto<SearchResult<BrandForSearchDto>>();

            var brands = _brandRepository.GetByParameters(userId, parametes);

            if (brands.TotalPageCount == 0)
            {
                result.Errors.Add("Nie znaleziono takich marek piw");
                return result;
            }

            if (parametes.PageNumber > brands.TotalPageCount)
            {
                result.Errors.Add($"Strona {parametes.PageNumber - 1} wykracza poza limit {brands.TotalPageCount - 1}");
                return result;
            }

            result.Object = brands;
            return result;
        }

        public ResponseDto<SearchResult<ReviewsForSearchDto>> GetReviews(long beerBrandId, ReviewSearchBindingModel parametes)
        {
            var result = new ResponseDto<SearchResult<ReviewsForSearchDto>>();

            var reviews = _brandRepository.GetReviewsByParameters(beerBrandId, parametes);

            if (reviews.TotalPageCount == 0)
            {
                result.Errors.Add("Nie znaleziono takich marek piw");
                return result;
            }

            if (parametes.PageNumber > reviews.TotalPageCount)
            {
                result.Errors.Add($"Strona {parametes.PageNumber - 1} wykracza poza limit {reviews.TotalPageCount - 1}");
                return result;
            }

            result.Object = reviews;
            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> AddOrEditRatingAsync(long beerBrandId, long userId, AddRatingBindingModel addRatingModel)
        {
            var result = new ResponseDto<BaseModelDto>();
            var brand = _brandRepository.Get(x => x.Id == beerBrandId);
            var yourRating = brand.Ratings.Find(rate => rate.Author?.Id == userId);
            if (yourRating != null)
            {
                yourRating.Rate = addRatingModel.Rating;
            }
            else
            {
                brand.Ratings.Add(new Rating
                {
                    Author = _userRepository.Get(u => u.Id == userId),
                    Brand = brand,
                    Rate = addRatingModel.Rating
                });
            }
            var updateResult = await _brandRepository.UpdateAsync(brand);
            if (!updateResult)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }

            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> DeleteRatingAsync(long beerBrandId, long userId)
        {
            var result = new ResponseDto<BaseModelDto>();
            var brand = _brandRepository.Get(x => x.Id == beerBrandId);
            var ratingToDelete = brand.Ratings.Find(rate => rate.Author.Id == userId);
            var deleteResult = await _ratingRepository.DeleteAsync(ratingToDelete);
            if (!deleteResult)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }

            return result;
        }
        
        public async Task<ResponseDto<BaseModelDto>> AddReviewAsync(long beerBrandId, long userId, AddReviewBindingModel addReviewModel)
        {
            var result = new ResponseDto<BaseModelDto>();
            var brand = _brandRepository.Get(x => x.Id == beerBrandId);
            var yourReview = brand.Reviews.FirstOrDefault(r => r.Author.Id == userId);
            if (yourReview != null)
            {
                result.Errors.Add("Już dodałeś recenzję tej marki.");
                return result;
            }
            var review = new Review
            {
                Author = _userRepository.Get(u => u.Id == userId),
                Brand = brand,
                Title = addReviewModel.Title,
                Content = addReviewModel.Content,
                AddedAt = DateTime.Now,
                EditedAt = null,
                IsDeleted = false
            };

            var addResult = await _brandRepository.InsertReviewAsync(review);
            if (!addResult)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }

            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> EditReviewAsync(long beerBrandId, long userId, EditReviewBindingModel editReviewModel, string userRole, long reviewId)
        {
            var result = new ResponseDto<BaseModelDto>();
            var brand = _brandRepository.Get(x => x.Id == beerBrandId);
            var yourReview = _brandRepository.GetReview(r => r.Id == reviewId);

            if (yourReview == null)
            {
                result.Errors.Add($"Recenzja o id {reviewId} nie istnieje");
                return result;
            }

            if (yourReview.Author.Id != userId && userRole != "Moderator" && userRole != "Administrator")
            {
                result.Errors.Add("Nie masz odpowiednich uprawnień, aby edytować tę recenzję");
                return result;
            }

            if (yourReview.Author.Id != userId && (userRole == "Moderator" || userRole == "Administrator"))
            {
                yourReview.Title = editReviewModel.Title;
                yourReview.Content = editReviewModel.Content;
                yourReview.EditedAt = DateTime.Now;
            }

            if (yourReview.Author.Id == userId)
            {
                yourReview.Title = editReviewModel.Title;
                yourReview.Content = editReviewModel.Content;
                yourReview.EditedAt = DateTime.Now;
            }

            var updateResult = await _brandRepository.UpdateReviewAsync(yourReview);
            if (!updateResult)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później.");
                return result;
            }

            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> DeleteReviewAsync(long beerBrandId, long userId, string userRole, long reviewId)
        {
            var result = new ResponseDto<BaseModelDto>();
            var brand = _brandRepository.Get(x => x.Id == beerBrandId);
            var yourReview = brand.Reviews.Find(review => review.Author.Id == userId);
            if (yourReview != null)
            {
                yourReview.IsDeleted = true;
            }
            else
            {
                if (userRole == "Moderator" || userRole == "Administrator")
                {
                    var review = _brandRepository.GetReview(r => r.Id == reviewId);
                    review.IsDeleted = true;
                }
                else
                {
                    result.Errors.Add("Nie masz odpowiednich uprawnień, aby usunąć tę recenzję");
                    return result;
                }
            }

            var updateResult = await _brandRepository.UpdateAsync(brand);
            if (!updateResult)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później.");
                return result;
            }

            return result;
        }

        public ResponseDto<ReviewDto> GetReview(long beerBrandId, long reviewId)
        {
            var review = _brandRepository.GetReview(r => r.Id == reviewId);
            var brand = _brandRepository.Get(b => b.Id == beerBrandId);
            var result = new ResponseDto<ReviewDto>();
            if (review == null || brand == null)
            {
                result.Errors.Add("Nie znaleziono obiektu");
                return result;
            }

            result.Object = new ReviewDto
            {
                BrandName = brand.Name,
                AddedDate = review.AddedAt,
                EditionDate = review.EditedAt,
                UserNickname = review.Author?.Email,
                Title = review.Title,
                Content = review.Content

            };

            return result;
        }
    }
}
