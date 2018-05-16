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
    public class KindService : IKindService
    {
        private readonly IKindRepository _kindRepository;
        private readonly IUserRepository _userRepository;

        public KindService(IKindRepository kindRepository, IUserRepository userRepository)
        {
            _kindRepository = kindRepository;
            _userRepository = userRepository;
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

        public async Task<ResponseDto<BaseModelDto>> AddKindAsync(long userId, KindBindingModel kindBindingModel)
        {
            var result = new ResponseDto<BaseModelDto>();

            Kind kind = new Kind
            {
                Name = kindBindingModel.Name,
                Description = kindBindingModel.Description,
                CreatedAt = kindBindingModel.CreatedAt,
                AddedBy = _userRepository.Get(u => u.Id == userId),
                AddedAt = DateTime.Now
            };
            var insertResult = await _kindRepository.InsertAsync(kind);
            if (!insertResult)
            {
                result.Errors.Add("Coś poszło nie tak! Spróbuj powonie później");
                return result;
            }

            return result;
        }
    }
}
