using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ExtensionMethods;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class KindRepository : IKindRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public KindRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Kind Get(Func<Kind, bool> function)
        {
            return _dbContext.Kinds.FirstOrDefault(function);
        }

        public SearchResult<KindDto> GetByParameters(SearchBindingModel parameters)
        {
            IEnumerable<Kind> kinds;

            if (!string.IsNullOrEmpty(parameters.Query))
            {
                kinds = _dbContext.Kinds.Where(k => !k.IsDeleted &&
                                                    (k.Name.Contains(parameters.Query) ||
                                                     k.CreatedAt.Value.ToLongDateString().Contains(parameters.Query) ||
                                                     k.CreatedAt.Value.ToShortDateString().Contains(parameters.Query) ||
                                                     k.Description.Contains(parameters.Query))).ToList();
            }
            else
            {
                kinds = _dbContext.Kinds.ToList();
            }

            var totalPages = (int) Math.Ceiling((decimal) kinds.Count() / parameters.Limit);

            var property = typeof(Kind).GetProperty(parameters.Sort.FirstCharToUpper());

            if (property == null)
            {
                var defaultParameters = new SearchBindingModel();
                property = typeof(Kind).GetProperty(defaultParameters.Sort);
            }

            kinds = parameters.Ascending
                ? kinds.OrderBy(k => property.GetValue(k))
                : kinds.OrderByDescending(k => property.GetValue(k));

            var result = new SearchResult<KindDto>();
            var kindsForSearch = new List<KindDto>();
            kinds.ToList().ForEach(k => 
                kindsForSearch.Add(new KindDto
                {
                    Name = k.Name,
                    CreationDate = k.CreatedAt,
                    Description = k.Description
                }
            ));

            result.CurrentPage = parameters.PageNumber;
            result.TotalPageCount = totalPages;
            result.Results = kindsForSearch;
            return result;
        }
    }
}
