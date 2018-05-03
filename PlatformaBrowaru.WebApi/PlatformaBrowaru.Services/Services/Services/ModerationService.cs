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
    public class ModerationService : IModerationService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IModerationRepository _moderationRepository;
        private readonly IUserRepository _userRepository;

        public ModerationService(IBrandRepository brandRepository, IUserRepository userRepository, IModerationRepository moderationRepository)
        {
            _brandRepository = brandRepository;
            _userRepository = userRepository;
            _moderationRepository = moderationRepository;
        }

        public async Task<ResponseDto<BaseModelDto>> AddForModeration(long userId, AddToModerationBindingModel addToModeration)
        {
            var result = new ResponseDto<BaseModelDto>();
            var brand = _brandRepository.Get(b => b.Id == addToModeration.BrandId);

            if (brand == null)
            {
                result.Errors.Add("Ta marka piwa nie istnieje");
                return result;
            }

            var exists = _moderationRepository.Get(m => m.Brand == brand) != null;
            if (exists)
            {
                result.Errors.Add("Ta marka piwa została już zgłoszona do moderacji");
                return result;
            }

            var brandToModerate = new BrandToModerate
            {
                Brand = brand,
                User = _userRepository.Get(u => u.Id == userId),
                Reason = addToModeration.Reason
            };

            var insertResult = await _moderationRepository.Insert(brandToModerate);

            if (!insertResult)
            {
                result.Errors.Add("Wystąpił nie oczekiwany błąd, spróbuj ponownie później");
                return result;
            }

            return result;
        }
    }
}
