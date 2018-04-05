using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseDto<LoginDto>> LoginAsync(LoginBindingModel loginModel);
        Task<ResponseDto<BaseModelDto>> RegisterAsync(RegisterBindingModel registerModel);
        Task<ResponseDto<BaseModelDto>> LogoutAsync(long userId);
    }
}
