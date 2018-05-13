using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseDto<LoginDto>> LoginAsync(LoginBindingModel loginModel);
        Task<ResponseDto<BaseModelDto>> RegisterAsync(RegisterBindingModel registerModel);
        Task<ResponseDto<BaseModelDto>> LogoutAsync(long userId);
        ResponseDto<GetUserDto> GetUser(long id);
        Task<ResponseDto<LoginDto>> GenerateTokensAsync(ApplicationUser user);
        ResponseDto<BaseModelDto> ActivateUser(Guid guid);
        ResponseDto<GetUserListDto> GetAllUsers();
        ResponseDto<BaseModelDto> DeleteUser(long id, string password);
        ResponseDto<BaseModelDto> ChangeEmail(long id, ChangeEmailBindingModel changeEmailModel);
        ResponseDto<BaseModelDto> ChangePassword(long id, ChangePasswordBindingModel changePasswordModel);
        Task<ResponseDto<BaseModelDto>> EditUserProfile(long userId, UserProfileBindingModel userProfile);
        string GetUserRole(long userId);
    }
}
