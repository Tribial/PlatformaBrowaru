using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Services.Services.Services;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ExtensionMethods;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;
using PlatformaBrowaru.WebApi.Controllers;
using Xunit;

namespace PlatformaBrowaru.Tests
{
    public class UserTests
    {
        [Fact]
        public void ShouldLoginWithSuccess()
        {
            var loginModel = new LoginBindingModel
            {
                Email = "jan.kowalski@mail.com",
                Password = "VeryStrongPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = loginModel.Password.ToHash(),
                Username = "jkowalski"
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.InsertRefreshTokenAsync(It.IsAny<RefreshToken>())).Returns(Task.FromResult(true));

            configuration.Setup(x => x.GetValue("Jwt:Key")).Returns("Some0Key0For0Security0Reasons");
            configuration.Setup(x => x.GetValue("Jwt:Issuer")).Returns("");
            configuration.Setup(x => x.GetValue("Jwt:ExpDays")).Returns("1");
            configuration.Setup(x => x.GetValue("Jwt:ExpRefreshToken")).Returns("7");

            var resultRaw = controller.LoginAsync(loginModel).Result;
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var loginResult = Assert.IsAssignableFrom<ResponseDto<LoginDto>>(result.Value);

            Assert.False(loginResult.ErrorOccured);
            Assert.NotNull(loginResult.Object);
        }

        [Fact]
        public void ShouldNotLoginOnInvalidEmail()
        {
            var loginModel = new LoginBindingModel
            {
                Email = "jan.kowalskiii@mail.com",
                Password = "VeryStrongPassword"
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);

            var resultRaw = controller.LoginAsync(loginModel).Result;
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var loginResult = Assert.IsAssignableFrom<ResponseDto<LoginDto>>(result.Value);

            Assert.True(loginResult.ErrorOccured);
            Assert.Contains("Złe dane logowania", loginResult.Errors);
        }

        [Fact]
        public void ShouldNotLoginOnInvalidPassword()
        {
            var loginModel = new LoginBindingModel
            {
                Email = "jan.kowalski@mail.com",
                Password = "VeryStrongPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "OtherStrongPassword".ToHash(),
                Username = "jkowalski"
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);

            var resultRaw = controller.LoginAsync(loginModel).Result;
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var loginResult = Assert.IsAssignableFrom<ResponseDto<LoginDto>>(result.Value);

            Assert.True(loginResult.ErrorOccured);
            Assert.Contains("Złe dane logowania", loginResult.Errors);
        }

        [Fact]
        public void ShouldNotLoginDeletedUser()
        {
            var loginModel = new LoginBindingModel
            {
                Email = "jan.kowalski@mail.com",
                Password = "VeryStrongPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = true,
                IsVerified = true,
                PasswordHash = loginModel.Password.ToHash(),
                Username = "jkowalski"
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);

            var resultRaw = controller.LoginAsync(loginModel).Result;
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var loginResult = Assert.IsAssignableFrom<ResponseDto<LoginDto>>(result.Value);

            Assert.True(loginResult.ErrorOccured);
            Assert.Contains("Złe dane logowania", loginResult.Errors);
        }

        [Fact]
        public void ShouldNotLoginNotActivatedAccount()
        {
            var loginModel = new LoginBindingModel
            {
                Email = "jan.kowalski@mail.com",
                Password = "VeryStrongPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = false,
                PasswordHash = loginModel.Password.ToHash(),
                Username = "jkowalski"
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);

            var resultRaw = controller.LoginAsync(loginModel).Result;
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var loginResult = Assert.IsAssignableFrom<ResponseDto<LoginDto>>(result.Value);

            Assert.True(loginResult.ErrorOccured);
            Assert.Contains($"Najpierw kliknij w wysłany link aktywacyjny na maila {user.Email}", loginResult.Errors);
        }

        [Fact]
        public void ShouldGetUserWithSuccess()
        {
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
            };


            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);

            var resultRaw = controller.GetUser(user.Id);
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var getUserResult = Assert.IsAssignableFrom<ResponseDto<GetUserDto>>(result.Value);

            Assert.False(getUserResult.ErrorOccured);
            Assert.NotNull(getUserResult.Object);
        }

        [Fact]
        public void ShouldGetUserFailedAndReturnErrorIfUserNotExist()
        {
            var guid = new Guid();
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
                Guid = guid
            };


            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);

            var resultRaw = controller.GetUser(user.Id);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var getUserResult = Assert.IsAssignableFrom<ResponseDto<GetUserDto>>(result.Value);

            Assert.True(getUserResult.ErrorOccured);
            Assert.Contains("Coś poszło nie tak. Użytkownik o podanym Id nie istnieje.", getUserResult.Errors);

        }

        [Fact]
        public async void ShouldReturnOkWhenRegisterSuccessfull()
        {
            var registerModel = new RegisterBindingModel
            {
                FirstName = "Arturo",
                LastName = "Karpinski",
                Username = "Arturo",
                Email = "mailmail@mail.com",
                Password = "1234567890",
                ConfirmPassword = "1234567890"

            };
            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);
            repository.Setup(x => x.Insert(It.IsAny<ApplicationUser>())).Returns(Task.FromResult(true));


            var result = await controller.RegisterAsync(registerModel);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(okResult.Value);

            Assert.False(resultValue.ErrorOccured);
        }

        [Fact]
        public async void ShouldReturnErrorIfUsernameExist()
        {
            var registerModel = new RegisterBindingModel
            {
                FirstName = "Arturo",
                LastName = "Karpinski",
                Username = "Arturo",
                Email = "mailmail@mail.com",
                Password = "1234567890",
                ConfirmPassword = "1234567890"

            };
            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);


            var result = await controller.RegisterAsync(registerModel);
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var resultValue = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(okResult.Value);

            Assert.True(resultValue.ErrorOccured);
            Assert.Contains("Podany przez ciebie login już istnieje", resultValue.Errors);
        }

        [Fact]
        public async void ShouldReturnErrorIfEmailExist()
        {
            var registerModel = new RegisterBindingModel
            {
                FirstName = "Arturo",
                LastName = "Karpinski",
                Username = "Arturo",
                Email = "mailmail@mail.com",
                Password = "1234567890",
                ConfirmPassword = "1234567890"

            };
            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);


            var result = await controller.RegisterAsync(registerModel);
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var resultValue = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(okResult.Value);

            Assert.True(resultValue.ErrorOccured);
            Assert.Contains("Podany przez ciebie login już istnieje", resultValue.Errors);
        }


        [Fact]
        public async void ShouldLogoutWithSuccess()
        {
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "VeryStrongPassword".ToHash(),
                Username = "jkowalski"
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);

            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.RemoveRefreshTokenAsync(It.IsAny<RefreshToken>())).Returns(Task.FromResult(true));

            var resultRaw = await controller.LogoutAsync();
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var logoutResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.False(logoutResult.ErrorOccured);
        }

        [Fact]
        public async void ShouldNotLogoutBecauseNotLoggedIn()
        {
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "VeryStrongPassword".ToHash(),
                Username = "jkowalski"
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>()))
                .Returns(Task.FromResult((RefreshToken) null));
            repository.Setup(x => x.RemoveRefreshTokenAsync(It.IsAny<RefreshToken>())).Returns(Task.FromResult(true));

            var resultRaw = await controller.LogoutAsync();
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var logoutResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(logoutResult.ErrorOccured);
            Assert.Contains("Nie jesteś zalogowany", logoutResult.Errors);
        }

        [Fact]
        public void ShouldActivateUserWithSuccess()
        {
            Guid guid = new Guid();
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = false,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
                Guid = guid
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);

            var resultRaw = controller.ActivateUser(user.Guid);
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var activateUserResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.False(activateUserResult.ErrorOccured);
        }

        [Fact]
        public void ShouldReturnErrorIfUserAlreadyActivated()
        {
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);

            var resultRaw = controller.ActivateUser(user.Guid);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var activateUserResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(activateUserResult.ErrorOccured);
            Assert.Contains("Twoje konto zostało już aktywowane", activateUserResult.Errors);
        }

        [Fact]
        public void ShouldDeleteUserWithSuccess()
        {
            var password = "abc";
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.DeleteUser(password);
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var deleteUserResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.False(deleteUserResult.ErrorOccured);
        }

        [Fact]
        public void ShouldDeleteUserFailedIfWrongPassword()
        {
            var password = "abcdeeee";
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.DeleteUser(password);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var deleteUserResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(deleteUserResult.ErrorOccured);
            Assert.Contains("Nieprawidłowe hasło", deleteUserResult.Errors);
        }
        [Fact]
        public void ShouldDeleteUserFailedAndReturnErrorIfUserNotExist()
        {
            var password = "abc";
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);

            var resultRaw = controller.DeleteUser(password);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var deleteUserResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(deleteUserResult.ErrorOccured);
            Assert.Contains("Coś poszło nie tak. Użytkownik nie istnieje.", deleteUserResult.Errors);
        }

        [Fact]
        public void ShouldDeleteUserFailedAndReturnErrorIfUserNotLogged()
        {
            var password = "abc";
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>()))
                .Returns(Task.FromResult((RefreshToken) null));

            var resultRaw = controller.DeleteUser(password);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var deleteUserResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(deleteUserResult.ErrorOccured);
            Assert.Contains("Nie jesteś zalogowany", deleteUserResult.Errors);
        }

        [Fact]
        public void ShouldDeleteUserFailedAndReturnErrorIfCouldNotSaveChanges()
        {
            var password = "abc";
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abc".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Save()).Returns(false);

            var resultRaw = controller.DeleteUser(password);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var deleteUserResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(deleteUserResult.ErrorOccured);
            Assert.Contains("Coś poszło nie tak, spróbuj ponownie później", deleteUserResult.Errors);
        }

        [Fact]
        public void ShouldChangeEmailWithSuccess()
        {
            var changeEmailModel = new ChangeEmailBindingModel
            {
                NewEmail = "janek69@gmail.com",
                ConfirmNewEmail = "janek69@gmail.com",
                Password = "abcPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abcPassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangeEmail(changeEmailModel);
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var changeEmailResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.False(changeEmailResult.ErrorOccured);
        }

        [Fact]
        public void ShouldChangeEmailFailedIfThisEmailAlreadyExist()
        {
            var changeEmailModel = new ChangeEmailBindingModel
            {
                NewEmail = "janek69@gmail.com",
                ConfirmNewEmail = "janek69@gmail.com",
                Password = "abcPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abcPassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangeEmail(changeEmailModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changeEmailResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changeEmailResult.ErrorOccured);
            Assert.Contains("Podany przez ciebie email już istnieje", changeEmailResult.Errors);
        }

        [Fact]
        public void ShouldChangeEmailFailedIfUserNotLoggedIn()
        {
            var changeEmailModel = new ChangeEmailBindingModel
            {
                NewEmail = "janek69@gmail.com",
                ConfirmNewEmail = "janek69@gmail.com",
                Password = "abcPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abcPassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>()))
                .Returns(Task.FromResult((RefreshToken) null));
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangeEmail(changeEmailModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changeEmailResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changeEmailResult.ErrorOccured);
            Assert.Contains("Nie jesteś zalogowany", changeEmailResult.Errors);
        }

        [Fact]
        public void ShouldChangeEmailFailedIfCouldNotGetUser()
        {
            var changeEmailModel = new ChangeEmailBindingModel
            {
                NewEmail = "janek69@gmail.com",
                ConfirmNewEmail = "janek69@gmail.com",
                Password = "abcPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abcPassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns((ApplicationUser) null);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangeEmail(changeEmailModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changeEmailResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changeEmailResult.ErrorOccured);
            Assert.Contains("Coś poszło nie tak. Użytkownik nie istnieje.", changeEmailResult.Errors);
        }

        [Fact]
        public void ShouldChangeEmailFailedIfCouldNotSaveChanges()
        {
            var changeEmailModel = new ChangeEmailBindingModel
            {
                NewEmail = "janek69@gmail.com",
                ConfirmNewEmail = "janek69@gmail.com",
                Password = "abcPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abcPassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);
            repository.Setup(x => x.Save()).Returns(false);

            var resultRaw = controller.ChangeEmail(changeEmailModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changeEmailResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changeEmailResult.ErrorOccured);
            Assert.Contains("Coś poszło nie tak, spróbuj ponownie później", changeEmailResult.Errors);
        }

        [Fact]
        public void ShouldChangeEmailFailedIfEmailsDoNotMatch()
        {
            var changeEmailModel = new ChangeEmailBindingModel
            {
                NewEmail = "janek69@gmail.com",
                ConfirmNewEmail = "NotNewEmail",
                Password = "abcPassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abcPassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangeEmail(changeEmailModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changeEmailResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changeEmailResult.ErrorOccured);
            Assert.Contains("Emaile się różnią", changeEmailResult.Errors);
        }

        [Fact]
        public void ShouldChangeEmailFailedIfPasswordIsNotCorrect()
        {
            var changeEmailModel = new ChangeEmailBindingModel
            {
                NewEmail = "janek69@gmail.com",
                ConfirmNewEmail = "janek69@gmail.com",
                Password = "MyFavouritePassword"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "abcPassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangeEmail(changeEmailModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changeEmailResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changeEmailResult.ErrorOccured);
            Assert.Contains("Nieprawidłowe hasło", changeEmailResult.Errors);
        }

        [Fact]
        public void ShouldChangePasswordWithSuccess()
        {
            var changePasswordModel = new ChangePasswordBindingModel
            {
                Password = "MyFavouritePassword",
                NewPassword = "ItsMyNewFavouritePassword!",
                ConfirmNewPassword = "ItsMyNewFavouritePassword!"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "MyFavouritePassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangePassword(changePasswordModel);
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var changePasswordResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.False(changePasswordResult.ErrorOccured);
        }

        [Fact]
        public void ShouldChangePasswordFailedIfCurrentPasswordIsNotCorrect()
        {
            var changePasswordModel = new ChangePasswordBindingModel
            {
                Password = "MyFavouritePassword123",
                NewPassword = "ItsMyNewFavouritePassword!",
                ConfirmNewPassword = "ItsMyNewFavouritePassword!"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "MyFavouritePassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)};

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangePassword(changePasswordModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changePasswordResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changePasswordResult.ErrorOccured);
            Assert.Contains("Twoje aktualne hasło jest nieprawidłowe", changePasswordResult.Errors);
        }

        [Fact]
        public void ShouldChangePasswordFailedIfPasswordsDoNotMatch()
        {
            var changePasswordModel = new ChangePasswordBindingModel
            {
                Password = "MyFavouritePassword",
                NewPassword = "ItsMyNewFavouritePassword!",
                ConfirmNewPassword = "ItsMyNewFavouritePassword!heh"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "MyFavouritePassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangePassword(changePasswordModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changePasswordResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changePasswordResult.ErrorOccured);
            Assert.Contains("Wartości w polu Nowe Hasło i Potwierdź Hasło muszą być takie same", changePasswordResult.Errors);
        }

        [Fact]
        public void ShouldChangePasswordFailedIfCouldNotGetUser()
        {
            var changePasswordModel = new ChangePasswordBindingModel
            {
                Password = "MyFavouritePassword",
                NewPassword = "ItsMyNewFavouritePassword!",
                ConfirmNewPassword = "ItsMyNewFavouritePassword!"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "MyFavouritePassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns((ApplicationUser)null);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Save()).Returns(true);

            var resultRaw = controller.ChangePassword(changePasswordModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changePasswordResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changePasswordResult.ErrorOccured);
            Assert.Contains("Coś poszło nie tak. Użytkownik nie istnieje.", changePasswordResult.Errors);
        }

        [Fact]
        public void ShouldChangePasswordFailedIfCouldNotSaveChanges()
        {
            var changePasswordModel = new ChangePasswordBindingModel
            {
                Password = "MyFavouritePassword",
                NewPassword = "ItsMyNewFavouritePassword!",
                ConfirmNewPassword = "ItsMyNewFavouritePassword!"
            };
            var user = new ApplicationUser
            {
                Email = "jan.kowalski@mail.com",
                CreatedAt = DateTime.Now.AddDays(-5),
                FirstName = "Jan",
                LastName = "Kowalski",
                Id = 1,
                IsDeleted = false,
                IsVerified = true,
                PasswordHash = "MyFavouritePassword".ToHash(),
                Username = "jkowalski",
                Guid = new Guid()
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var emailService = new Mock<IEmailService>();
            var service = new UserService(repository.Object, configuration.Object, emailService.Object);
            var controller = new UsersController(service);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

            repository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(
                Task.FromResult(
                    new RefreshToken
                    {
                        Token = "AnyToken",
                        TokenExpirationDate = DateTime.Now
                    }
                )
            );
            repository.Setup(x => x.Save()).Returns(false);

            var resultRaw = controller.ChangePassword(changePasswordModel);
            var result = Assert.IsType<BadRequestObjectResult>(resultRaw);
            var changePasswordResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.True(changePasswordResult.ErrorOccured);
            Assert.Contains("Coś poszło nie tak, spróbuj ponownie później", changePasswordResult.Errors);
        }
    }
}
