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
            if (_kindRepository.Get(x => x.Name == kind.Name) != null)
            {
                result.Errors.Add("Gatunek o podanej nazwie już istnieje.");
                return result;
            }
            var insertResult = await _kindRepository.InsertAsync(kind);
            if (!insertResult)
            {
                result.Errors.Add("Coś poszło nie tak! Spróbuj powonie później");
                return result;
            }

            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> EditKindAsync(long userId, KindBindingModel kindBindingModel, long id)
        {

            var result = new ResponseDto<BaseModelDto>();
            var kindAlreadyExist = _kindRepository.Get(x => x.Name == kindBindingModel.Name && x.Id != id);
            if (kindAlreadyExist != null)
            {
                result.Errors.Add("Gatunek o podanej nazwie już istnieje.");
                return result;
            }

            var kind = _kindRepository.Get(x => x.Id == id);
            kind.Name = kindBindingModel.Name;
            kind.Description = kindBindingModel.Description;
            kind.CreatedAt = kindBindingModel.CreatedAt;
            kind.EditedBy = _userRepository.Get(x => x.Id == userId);
            kind.EditedAt = DateTime.Now;

            
            var updateResult = await _kindRepository.UpdateAsync(kind);
            if (!updateResult)
            {
                result.Errors.Add("Coś poszło nie tak! Spróbuj powonie później");
                return result;
            }

            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> DeleteKindAsync(long userId, long id)
        {

            var result = new ResponseDto<BaseModelDto>();
            var user = _userRepository.Get(x => x.Id == userId);
            if (user.Role == "User")
            {
                result.Errors.Add("Nie masz uprawnień, aby wykonać tę operację.");
                return result;
            }
            var kind = _kindRepository.Get(x => x.Id == id);
            kind.IsDeleted = true;


            var updateResult = await _kindRepository.UpdateAsync(kind);
            if (!updateResult)
            {
                result.Errors.Add("Coś poszło nie tak! Spróbuj powonie później");
                return result;
            }

            return result;
        }
    }
}
