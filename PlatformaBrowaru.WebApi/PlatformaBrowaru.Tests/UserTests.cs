using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    }
}
