using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ExtensionMethods;
using PlatformaBrowaru.Share.ModelsDto;


namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> InsertAsync(Brand brand)
        {
            await _dbContext.Brands.AddAsync(brand);
            return await SaveAsync();
        }

        public Brand Get(Func<Brand, bool> function)
        {
            var result = _dbContext.Brands
                .Include(b => b.BrandWrappings)
                .Include(b => b.BrandSeasons)
                .Include(b => b.BrandFermentationTypes)
                .Include(b => b.BrandBrewingMethods)
                .Include(b => b.Reviews).ThenInclude(b => b.Author)
                .Include(b => b.Ratings).ThenInclude(r => r.Author)
                .Include(b => b.Kind)
                .FirstOrDefault(function);
            return result;
        }

        public Review GetReview(Func<Review, bool> function)
        {
            return _dbContext.Reviews.Include(r => r.Author).FirstOrDefault(function);
        }

        public SearchResult<ReviewsForSearchDto> GetReviewsByParameters(long beerBrandId, ReviewSearchBindingModel parameters)
        {
            IEnumerable<Review> reviews;

            if (parameters.Query != null)
            {
                reviews = _dbContext.Reviews.Include(b => b.Author).Where(b =>
                    b.Brand.Id == beerBrandId && b.IsDeleted == false &&
                    (b.Title.Contains(parameters.Query) ||
                    b.Content.Contains(parameters.Query) ||
                     b.Author.FirstName.Contains(parameters.Query) ||
                     b.Author.LastName.Contains(parameters.Query))
                ).ToList();
            }
            else
            {
                reviews = _dbContext.Reviews.Include(b => b.Author).Where(b => b.Brand.Id == beerBrandId).ToList();
            }

            var totalPages = (int)Math.Ceiling((decimal)reviews.Count() / parameters.Limit);

            var property = typeof(Review).GetProperty(parameters.Sort.FirstCharToUpper());

            if (property == null)
            {
                var defaultParameters = new ReviewSearchBindingModel();
                property = typeof(Review).GetProperty(defaultParameters.Sort);
            }



            reviews = parameters.Ascending
                ? reviews.OrderBy(b => property.GetValue(b))
                : reviews.OrderByDescending(b => property.GetValue(b));

            reviews = reviews.Skip(parameters.Limit * (parameters.PageNumber - 1)).Take(parameters.Limit);

            var result = new SearchResult<ReviewsForSearchDto>();
            var reviewsForSearch = new List<ReviewsForSearchDto>();
            reviews.ToList().ForEach(b =>
                reviewsForSearch.Add(new ReviewsForSearchDto
                {
                    Id = b.Id,
                    UserNickname = b.Author.Email,
                    Title = b.Title,
                    AddedAt = b.AddedAt
                }
            ));

            result.CurrentPage = parameters.PageNumber;
            result.TotalPageCount = totalPages;
            result.Results = reviewsForSearch;

            return result;
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _dbContext.Reviews.Update(review);
            return await SaveAsync();
        }

        public async Task<bool> InsertReviewAsync(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(Brand brand)
        {
            _dbContext.Brands.Update(brand);
            return await SaveAsync();
        }

        public SearchResult<BrandForSearchDto> GetByParameters(long userId, SearchBindingModel parameters)
        {
            IEnumerable<Brand> brands;

            if (parameters.Query != null)
            {
                brands = _dbContext.Brands.Include(b => b.Kind).Include(b => b.Ratings).Where(b =>
                    b.IsAccepted &&
                    (b.Name.Contains(parameters.Query) ||
                    b.Kind.Name.Contains(parameters.Query))
                ).ToList();
            }
            else
            {
                brands = _dbContext.Brands.Include(b => b.Kind).Include(b => b.Ratings).Where(b => b.IsAccepted).ToList();
            }

            var totalPages = (int) Math.Ceiling((decimal) brands.Count() / parameters.Limit);

            var property = typeof(Brand).GetProperty(parameters.Sort.FirstCharToUpper());

            if (property == null)
            {
                var defaultParameters = new SearchBindingModel();
                property = typeof(Brand).GetProperty(defaultParameters.Sort);
            }

            

            brands = parameters.Ascending
                ? brands.OrderBy(b => property.GetValue(b))
                : brands.OrderByDescending(b => property.GetValue(b));

            brands = brands.Skip(parameters.Limit * (parameters.PageNumber - 1)).Take(parameters.Limit);

            var result = new SearchResult<BrandForSearchDto>();
            var brandsForSearch = new List<BrandForSearchDto>();
            brands.ToList().ForEach(b => 
                brandsForSearch.Add(new BrandForSearchDto
                {
                    Id = b.Id,
                    Alcohol = b.AlcoholAmountPercent,
                    KindName = b.Kind == null ? 
                        "Brak rodzaju" : 
                        (b.Kind.IsDeleted ? 
                            "Rodzaj usunięty" : 
                            b.Kind?.Name),
                    Name = b.Name,
                    Rate = b.Ratings.Count != 0 ? b.Ratings.Sum(x => x.Rate) / b.Ratings.Count : -1,
                    UserRate = b.Ratings.FirstOrDefault(r => r.Author?.Id == userId)
                }
            ));

            result.CurrentPage = parameters.PageNumber;
            result.TotalPageCount = totalPages;
            result.Results = brandsForSearch;

            return result;
        }
    }
}
