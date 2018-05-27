using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Interfaces
{
    public interface IKindService
    {
        ResponseDto<SearchResult<KindDto>> GetKinds(SearchBindingModel parameters);
        Task<ResponseDto<BaseModelDto>> AddKindAsync(long userId, KindBindingModel kindBindingModel);
        Task<ResponseDto<BaseModelDto>> EditKindAsync(long userId, KindBindingModel kindBindingModel, long id);
        Task<ResponseDto<BaseModelDto>> DeleteKindAsync(long userId, long id);
    }
}
