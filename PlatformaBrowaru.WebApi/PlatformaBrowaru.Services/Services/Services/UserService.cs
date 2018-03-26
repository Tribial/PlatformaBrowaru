using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ExtensionMethods;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.Services.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IConfigurationService configurationService)
        {
            _userRepository = userRepository;
            _configurationService = configurationService;
        }

        public async Task<ResponseDto<LoginDto>> LoginAsync(LoginBindingModel loginModel)
        {
            var result = new ResponseDto<LoginDto>();

            loginModel.Password = loginModel.Password.ToHash();

            var user = _userRepository.Get(u => u.Email == loginModel.Email);

            if (user?.PasswordHash != loginModel.Password || user?.IsDeleted == true)
            {
                result.Errors.Add("Złe dane logowania");
                return result;
            }

            if (!user.IsVerified)
            {
                result.Errors.Add($"Najpierw kliknij w wysłany link aktywacyjny na maila {user.Email}");
                return result;
            }

            result = await GenerateTokensAsync(user);
            return result;
        }

        private async Task<ResponseDto<LoginDto>> GenerateTokensAsync(ApplicationUser user)
        {
            var secretKey = _configurationService.GetValue("Jwt:Key");
            var issuer = _configurationService.GetValue("Jwt:Issuer");
            var expirationDate = DateTime.Now.AddDays(Convert.ToDouble(_configurationService.GetValue("Jwt:ExpDays")));

            var result = new ResponseDto<LoginDto>()
            {
                Object = new LoginDto()
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: expirationDate,
                signingCredentials: creds
            );

            result.Object.Token = new JwtSecurityTokenHandler().WriteToken(token);
            result.Object.TokenExpirationDate = expirationDate;

            var refreshTokenRaw = user.Email + Guid.NewGuid() + user.Id + DateTime.Now.ToShortDateString() + secretKey;
            var refreshToken = new RefreshToken()
            {
                UserId = user.Id,
                Token = refreshTokenRaw.ToHash(),
                TokenExpirationDate =
                    DateTime.Now.AddDays(Convert.ToDouble(_configurationService.GetValue("Jwt:ExpRefreshToken")))
            };

            result.Object.RefreshToken = refreshToken.Token;
            result.Object.RefreshTokenExpirationDate = refreshToken.TokenExpirationDate;

            var existingRefreshToken = await _userRepository.GetRefreshTokenAsync(user.Id);

            if (existingRefreshToken != null)
            {
                var removingResult = await _userRepository.RemoveRefreshTokenAsync(existingRefreshToken);
                if (!removingResult)
                {
                    result.Errors.Add("Wystąpił problem przy logowaniu");
                    return result;
                }
            }

            var insertResult = await _userRepository.InsertRefreshTokenAsync(refreshToken);

            if (insertResult) return result;

            result.Errors.Add("Wystąpił problem przy logowaniu");
            return result;
        }
    }
}