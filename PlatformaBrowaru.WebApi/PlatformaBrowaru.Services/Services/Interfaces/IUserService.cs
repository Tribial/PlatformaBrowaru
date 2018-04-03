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
        ResponseDto<BaseModelDto> Register(RegisterBindingModel registerModel);
    }
}
