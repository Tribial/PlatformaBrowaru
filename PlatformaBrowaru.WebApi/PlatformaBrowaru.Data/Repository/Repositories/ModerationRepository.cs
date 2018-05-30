using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ExtensionMethods;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class ModerationRepository : IModerationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ModerationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Insert(BrandToModerate brandToModerate)
        {
            await _dbContext.BrandsToModerate.AddAsync(brandToModerate);
            return await SaveAsync();
        }

        public BrandToModerate Get(Func<BrandToModerate, bool> function)
        {
            return _dbContext.BrandsToModerate.FirstOrDefault(function);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public SearchResult<BrandToModerateDto> GetByParameters(SearchBindingModel parameters)
        {
            IEnumerable<Brand> brands;

            if (!string.IsNullOrEmpty(parameters.Query))
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

            var brandsToModerate = _dbContext.BrandsToModerate.ToList();

            brands = brands.Where(b => brandsToModerate.Select(btm => btm.Id).ToList().Contains(b.Id));

            if (!string.IsNullOrEmpty(parameters.Query))
            {
                brands = brands.Concat(_dbContext.Brands.Include(b => b.Kind).Include(b => b.Ratings).Where(b =>
                    !b.IsAccepted &&
                    (b.Name.Contains(parameters.Query) ||
                     b.Kind.Name.Contains(parameters.Query))
                ).ToList());
            }
            else
            {
                brands = brands.Concat(_dbContext.Brands.Include(b => b.Kind).Include(b => b.Ratings).Where(b => !b.IsAccepted).ToList());
            }

            var totalPages = (int)Math.Ceiling((decimal)brands.Count() / parameters.Limit);

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

            var result = new SearchResult<BrandToModerateDto>();
            var brandsForSearch = new List<BrandToModerateDto>();
            brands.ToList().ForEach(b =>
                brandsForSearch.Add(new BrandToModerateDto
                {
                    Id = b.Id,
                    Alcohol = b.AlcoholAmountPercent,
                    Name = b.Name,
                    Rate = b.Ratings.Count != 0 ? b.Ratings.Sum(x => x.Rate) / b.Ratings.Count : -1,
                    UserNickname = b.AddedBy?.Email,
                    Status = b.IsAccepted ? "Do moderacji" : "Do akceptacji"
                }
            ));

            result.CurrentPage = parameters.PageNumber;
            result.TotalPageCount = totalPages;
            result.Results = brandsForSearch;

            return result;
        }
    }
}
