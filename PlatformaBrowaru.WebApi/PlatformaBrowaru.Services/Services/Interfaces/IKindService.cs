using System;
using System.Collections.Generic;
using System.Text;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Interfaces
{
    public interface IKindService
    {
        ResponseDto<SearchResult<KindDto>> GetKinds(SearchBindingModel parameters);
    }
}
