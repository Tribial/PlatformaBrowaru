using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IKindRepository
    {
        Kind Get(Func<Kind, bool> function);
        Task<bool> InsertAsync(Kind kind);
        Task<bool> SaveAsync();
        SearchResult<KindDto> GetByParameters(SearchBindingModel parameters);
    }
}
