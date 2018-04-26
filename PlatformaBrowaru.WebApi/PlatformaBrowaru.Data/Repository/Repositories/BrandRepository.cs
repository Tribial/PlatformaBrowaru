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
            var result = _dbContext.Brands.FirstOrDefault(function);
            return result;
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

        public SearchResult<BrandForSearchDto> GetByParameters(long userId, BrandSearchBindingModel parameters)
        {
            IEnumerable<Brand> brands;

            if (parameters.Query != null)
            {
                brands = _dbContext.Brands.Include(b => b.Kind).Include(b => b.BrandProduction).Include(b => b.Ratings).Where(b =>
                    b.Name.Contains(parameters.Query) ||
                    b.BrandProduction.ProducedBy.Owner.Name.Contains(parameters.Query) ||
                    b.Kind.Name.Contains(parameters.Query)
                ).ToList();
            }
            else
            {
                brands = _dbContext.Brands.Include(b => b.Kind).Include(b => b.BrandProduction).ToList();
            }

            var totalPages = (int) Math.Ceiling((decimal) brands.Count() / parameters.Limit);

            var property = typeof(Brand).GetProperty(parameters.Sort.FirstCharToUpper());

            if (property == null)
            {
                BrandSearchBindingModel defaultParameters = new BrandSearchBindingModel();
                property = typeof(Brand).GetProperty(defaultParameters.Sort);
            }

            brands = parameters.Ascending
                ? brands.OrderBy(b => property.GetValue(b))
                : brands.OrderByDescending(b => property.GetValue(b));

            brands = brands.Skip(parameters.Limit * (parameters.PageNumber - 1)).Take(parameters.Limit);

            var result = new SearchResult<BrandForSearchDto>();
            var brandsForSearch = new List<BrandForSearchDto>();
            foreach (var brand in brands)
            {
                var toAdd = new BrandForSearchDto();

                toAdd.Alcohol = brand.AlcoholAmountPercent;
                var brandProduction = _dbContext.BrandProductions.Include(b => b.ProducedBy)
                    .First(b => b.Id == brand.BrandProduction.Id);
                var brewery = _dbContext.Breweries.Include(b => b.Owner)
                    .First(b => b.Id == brandProduction.ProducedBy.Id);
                var brewingGroup = _dbContext.BrewingGroups.First(b => b.Id == brewery.Owner.Id);
                toAdd.BrewingGroup = new SimpleBrewingGroupDto
                {
                    Id = brewingGroup.Id,
                    Name = brewingGroup.Name
                };
                toAdd.KindName = brand.Kind.Name;
                toAdd.Name = brand.Name;
                toAdd.Rate = brand.Ratings.Sum(x => x.Rate) / brand.Ratings.Count;
                toAdd.UserRate = brand.Ratings.FirstOrDefault(r => r.Author?.Id == userId);

            }

            result.CurrentPage = parameters.PageNumber;
            result.TotalPageCount = totalPages;
            result.Results = brandsForSearch;

            return result;
        }
    }
}
