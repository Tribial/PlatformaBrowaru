using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IConfigurationService configurationService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _configurationService = configurationService;
            _emailService = emailService;
        }

        public async Task<ResponseDto<LoginDto>> LoginAsync(LoginBindingModel loginModel)
        {
            var result = new ResponseDto<LoginDto>();

            loginModel.Password = loginModel.Password.ToHash();

            if (!_userRepository.Exists(u => u.Email == loginModel.Email))
            {
                result.Errors.Add("Złe dane logowania");
                return result;
            }

            var user = _userRepository.Get(u => u.Email == loginModel.Email);

            if (user.PasswordHash != loginModel.Password || user.IsDeleted == true)
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
            result.Object.Id = user.Id;
            result.Object.Email = user.Email;
            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> LogoutAsync(long userId)
        {
            var result = new ResponseDto<BaseModelDto>();

            if (!_userRepository.Exists(u => u.Id == userId))
            {
                result.Errors.Add("Taki użytkownik nie istnieje");
                return result;
            }

            var token = await _userRepository.GetRefreshTokenAsync(userId);

            if (token == null)
            {
                result.Errors.Add("Nie jesteś zalogowany");
                return result;
            }

            var logoutResult = await _userRepository.RemoveRefreshTokenAsync(token);

            if (!logoutResult)
            {
                result.Errors.Add("Wystąpił problem po stronie serwera, spróbuj ponownie za chwile");
            }

            return result;

        }

        public async Task<ResponseDto<LoginDto>> GenerateTokensAsync(ApplicationUser user)
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
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Hash, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? ""), 
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

        public async Task<ResponseDto<BaseModelDto>> RegisterAsync(RegisterBindingModel registerModel)
        {
            var result = new ResponseDto<BaseModelDto>
            {
                Errors = new List<string>()

            };

            var userWithSameUsernmeAlreadyExists = _userRepository.Exists(x => x.Username == registerModel.Username);

            if (userWithSameUsernmeAlreadyExists)
            {
                result.Errors.Add("Podany przez ciebie login już istnieje");
                return result;

            }

            var userWithSameEmailAlreadyExists = _userRepository.Exists(x => x.Email == registerModel.Email);

            if (userWithSameEmailAlreadyExists)
            {
                result.Errors.Add("Podany przez ciebie email już istnieje");
                return result;
            }

            int age = DateTime.Today.Year - registerModel.DateOfBirth.Year;
            if (DateTime.Today < registerModel.DateOfBirth.AddYears(age)) age--;

            if (age < 18)
            {
                result.Errors.Add("Musisz mieć ukończone 18 lat.");
                return result;
            }

            var user = new ApplicationUser()
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Username = registerModel.Username,
                Email = registerModel.Email,
                PasswordHash = registerModel.Password.ToHash(),
                CreatedAt = DateTime.Now,
                Guid = Guid.NewGuid(),
                Description = registerModel.Description,
                Role = registerModel.Role,
                Gender = registerModel.Gender,
                DateOfBirth = registerModel.DateOfBirth
            };

            var userRepository = await _userRepository.Insert(user);
            if (!userRepository)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }


            await _emailService.SendEmail(user.Email, "Platforma Browaru - aktywacja",
                $"Witaj {user.Username}!\n Aby aktywować swoje konto kliknij w poniższy link:\n http://localhost:3000/Users/Activate/" +
                user.Guid);

            return result;
        }

        public ResponseDto<GetUserDto> GetUser(long id)
        {
            var user = _userRepository.Get(x => x.Id == id);
            var result = new ResponseDto<GetUserDto>
            {
                Errors = new List<string>(),
                Object = new GetUserDto()
            };

            var userWithThisIdExist = _userRepository.Exists(x => x.Id == id);

            if (!userWithThisIdExist)
            {
                result.Errors.Add("Coś poszło nie tak. Użytkownik o podanym Id nie istnieje.");
                return result;
            }
            result.Object.FirstName = user.FirstName;
            result.Object.LastName = user.LastName;
            result.Object.Email = user.Email;
            result.Object.Username = user.Username;
            result.Object.Description = user.Description;
            result.Object.CreatedAt = user.CreatedAt;
            result.Object.DateOfBirth = user.DateOfBirth;
            result.Object.Gender = user.Gender;
            if (user.IsVerified && !user.IsDeleted)
                result.Object.Status = "Aktywowany";
            else if (user.IsDeleted)
                result.Object.Status = "Usunięty";
            else
                result.Object.Status = "Zarejestrowany";

            return result;
        }

        public ResponseDto<BaseModelDto> ActivateUser(Guid guid)
        {
            var user = _userRepository.Get(x => x.Guid == guid);
            var result = new ResponseDto<BaseModelDto>
            {
                Errors = new List<string>(),
            };
            if (user is null)
            {
                result.Errors.Add("Nieprawidłowy link!");
                return result;
            }

            if (user.IsVerified)
            {
                result.Errors.Add("Twoje konto zostało już aktywowane");
                return result;
            }

            var userWithThisGuidExist = _userRepository.Exists(x => x.Guid == guid);
            if (!userWithThisGuidExist)
            {
                result.Errors.Add("Coś poszło nie tak. Użytkownik nie istnieje.");
                return result;
            }

            user.IsVerified = true;
            _userRepository.Save();

            return result;
        }

        public ResponseDto<GetUserListDto> GetAllUsers()
        {
            var user = _userRepository.GetAll();
            var result = new ResponseDto<GetUserListDto>
            {
                Errors = new List<string>(),
                Object = new GetUserListDto()
            };

            foreach (var element in user)
            {
                var userObject = new GetUserDto()
                {
                    FirstName = element.FirstName,
                    LastName = element.LastName,
                    Email = element.Email,
                    Username = element.Username
                };

                result.Object.ListUsers.Add(userObject);
            }
            return result;
        }

        public ResponseDto<BaseModelDto> DeleteUser(long id, string password)
        {
            var result = new ResponseDto<BaseModelDto>
            {
                Errors = new List<string>()
            };
            var userExists = _userRepository.Exists(x => x.Id == id);
            if (!userExists)
            {
                result.Errors.Add("Coś poszło nie tak. Użytkownik nie istnieje.");
                return result;
            }

            var userToDelete = _userRepository.Get(x => x.Id == id);

            if (password.ToHash() != userToDelete.PasswordHash)
            {
                result.Errors.Add("Nieprawidłowe hasło");
                return result;
            }
            userToDelete.IsDeleted = true;

            var token = _userRepository.GetRefreshTokenAsync(id).Result;

            if (token == null)
            {
                result.Errors.Add("Nie jesteś zalogowany");
                return result;
            }
            var userRepository = _userRepository.Save();
            if (!userRepository)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }

            return result;
        }

        public ResponseDto<BaseModelDto> ChangeEmail(long id, ChangeEmailBindingModel changeEmailModel)
        {
            var result = new ResponseDto<BaseModelDto>
            {
                Errors = new List<string>()
            };
            
            var user = _userRepository.Get(x => x.Id == id);
            if (user == null)
            {
                result.Errors.Add("Coś poszło nie tak. Użytkownik nie istnieje.");
                return result;
            }
            var token = _userRepository.GetRefreshTokenAsync(id).Result;

            if (token == null)
            {
                result.Errors.Add("Nie jesteś zalogowany");
                return result;
            }
            if (changeEmailModel.NewEmail != changeEmailModel.ConfirmNewEmail)
            {
                result.Errors.Add("Emaile się różnią");
                return result;
            }
            if (changeEmailModel.Password.ToHash() != user.PasswordHash)
            {
                result.Errors.Add("Nieprawidłowe hasło");
                return result;
            }
            var userWithThisEmailExists = _userRepository.Exists(x => x.Email == changeEmailModel.NewEmail);
            if (userWithThisEmailExists)
            {
                result.Errors.Add("Podany przez ciebie email już istnieje");
                return result;
            }
            user.Email = changeEmailModel.NewEmail;
            user.IsVerified = false;
            var userRepository = _userRepository.Save();
            if (!userRepository)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }

            _emailService.SendEmail(user.Email, "Platforma Browaru - aktywacja",
                $"Witaj {user.FirstName}!\n Aby aktywować swoje konto kliknij w poniższy link:\n http://localhost:3000/Users/Activate/" +
                user.Guid);

            return result;
        }

        public ResponseDto<BaseModelDto> ChangePassword(long id, ChangePasswordBindingModel changePasswordModel)
        {
            var result = new ResponseDto<BaseModelDto>
            {
                Errors = new List<string>()
            };

            var user = _userRepository.Get(x => x.Id == id);
            if (user == null)
            {
                result.Errors.Add("Coś poszło nie tak. Użytkownik nie istnieje.");
                return result;
            }

            if (changePasswordModel.Password.ToHash() != user.PasswordHash)
            {
                result.Errors.Add("Twoje aktualne hasło jest nieprawidłowe");
                return result;
            }
            if (changePasswordModel.NewPassword != changePasswordModel.ConfirmNewPassword)
            {
                result.Errors.Add("Wartości w polu Nowe Hasło i Potwierdź Hasło muszą być takie same");
                return result;
            }
            user.PasswordHash = changePasswordModel.NewPassword.ToHash();

            var userRepository = _userRepository.Save();
            if (!userRepository)
            {
                result.Errors.Add("Coś poszło nie tak, spróbuj ponownie później");
                return result;
            }

            return result;
        }

        public async Task<ResponseDto<BaseModelDto>> EditUserProfile(long userId, UserProfileBindingModel userProfile)
        {
            var result = new ResponseDto<BaseModelDto>();
            var usernameExists = _userRepository.Exists(u => u.Username == userProfile.Username);

            if (usernameExists)
            {
                result.Errors.Add("Ta nazwa użytkownika jest już zajęta");
                return result;
            }

            var user = _userRepository.Get(u => u.Id == userId);

            user.FirstName = userProfile.FirstName;
            user.LastName = userProfile.LastName;
            user.Username = userProfile.Username;

            var updateResult = await _userRepository.Update(user);

            if (!updateResult)
            {
                result.Errors.Add("Wystąpił nieoczekiwany błąd, spróbuj ponownie później");
            }

            return result;

        }

        public string GetUserRole(long userId)
        {
            var user = _userRepository.Get(u => u.Id == userId);
            return user.Role;
        }
    }
}