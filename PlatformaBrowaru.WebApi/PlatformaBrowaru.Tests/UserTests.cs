﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
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
            var configuration =  new Mock<IConfigurationService>();
            var service = new UserService(repository.Object, configuration.Object);
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
            var service = new UserService(repository.Object, configuration.Object);
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
            var service = new UserService(repository.Object, configuration.Object);
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
            var service = new UserService(repository.Object, configuration.Object);
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
            var service = new UserService(repository.Object, configuration.Object);
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
        public void ShouldReturnOkIfUserIdExist()
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
                Username = "jkowalski"
            };

            var repository = new Mock<IUserRepository>();
            var configuration = new Mock<IConfigurationService>();
            var service = new UserService(repository.Object, configuration.Object);
            var controller = new UsersController(service);

            repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);

            var resultRaw = controller.GetUser(user.Id);
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var getUserResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.False(getUserResult.ErrorOccured);
        }

        //[Fact]
        //public void ShouldReturnOkWhenRegisterSuccessfull()
        //{
        //    var registerModel = new RegisterBindingModel
        //    {
        //        FirstName="Arturo",
        //        LastName="Karpinski",
        //        Username="Arturo",
        //        Email="mail@mail.com",
        //        Password="1234567890",
        //        ConfirmPassword="1234567890"

        //    };
        //    var repository = new Mock<IUserRepository>();
        //    var configuration = new Mock<IConfigurationService>();
        //    var service = new UserService(repository.Object, configuration.Object);
        //    var controller = new UsersController(service);

        //    repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(false);



        //    var result = controller.RegisterAsync(registerModel).Result;
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var resultValue = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(okResult.Value);

        //    Assert.False(resultValue.ErrorOccured);
        //}

        //[Fact]
        //public void ShouldLogoutSuccessful()
        //{
        //    var user = new ApplicationUser
        //    {
        //        Email = "jan.kowalski@mail.com",
        //        CreatedAt = DateTime.Now.AddDays(-5),
        //        FirstName = "Jan",
        //        LastName = "Kowalski",
        //        Id = 1,
        //        IsDeleted = false,
        //        IsVerified = false,
        //        PasswordHash = "SomePassword".ToHash(),
        //        Username = "jkowalski"
        //    };

        //    var userIdAsString = "1";

        //    var repository = new Mock<IUserRepository>();
        //    var configuration = new Mock<IConfigurationService>();
        //    var service = new UserService(repository.Object, configuration.Object);
        //    var token = service
        //    var controller = new UsersController(service);

        //    repository.Setup(x => x.Exists(It.IsAny<Func<ApplicationUser, bool>>())).Returns(true);
        //    repository.Setup(x => x.GetRefreshTokenAsync(It.IsAny<long>())).Returns(Task.FromResult(token));

        //    var contextMock = new Mock<HttpContext>();
        //    var userIdClaim = new Claim(ClaimTypes.Sid, userIdAsString);

        //    contextMock.Setup(x => x.User.Claims).Returns();

        //    controller.ControllerContext.HttpContext = contextMock.Object;

        //    var resultRaw = controller.LoginAsync(loginModel).Result;
        //    var result = Assert.IsType<OkObjectResult>(resultRaw);
        //    var loginResult = Assert.IsAssignableFrom<ResponseDto<LoginDto>>(result.Value);

        //    Assert.False(loginResult.ErrorOccured);
        //    Assert.NotNull(loginResult.Object);
        //}
    }
}
