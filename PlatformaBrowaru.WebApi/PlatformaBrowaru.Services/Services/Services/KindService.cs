using System;
using System.Collections.Generic;
using System.Text;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Services
{
    public class KindService : IKindService
    {
        private readonly IKindRepository _kindRepository;

        public KindService(IKindRepository kindRepository)
        {
            _kindRepository = kindRepository;
        }

        public ResponseDto<SearchResult<KindDto>> GetKinds(SearchBindingModel parameters)
        {
            var result = new ResponseDto<SearchResult<KindDto>>();
            var kinds = _kindRepository.GetByParameters(parameters);

            if (kinds.TotalPageCount == 0)
            {
                result.Errors.Add("Nie znaloziono takich gatunków");
                return result;
            }

            if (parameters.PageNumber > kinds.TotalPageCount)
            {
                result.Errors.Add($"Strona {parameters.PageNumber} wykracza poza limit {kinds.TotalPageCount}");
                return result;
            }

            result.Object = kinds;
            return result;
        }
    }
}
