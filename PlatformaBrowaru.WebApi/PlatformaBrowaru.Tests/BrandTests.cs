using System;
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
    public class BrandTests
    {
        [Fact]
        public void ShouldAddBrandSuccessful()
        {
            var newBrand = new BrandBindingModel
            {
                AlcoholAmountPercent = 5,
                BrewingMethodIds = new List<long>
                {
                    1, 3
                },
                Color = "Ciemne",
                CreationDate = DateTime.Now.AddMonths(-4),
                Description = "Browar do testu",
                ExtractPercent = 14.5M,
                FermentationTypeIds = new List<long>
                {
                    2, 3
                },
                HopIntensity = 2,
                Ingredients = "Woda, chmiel, pszenica",
                Name = "Heineken Porter",
                TasteFullness = 5,
                Sweetness = 1,
                KindId = 14,
                IsFiltered = true,
                IsPasteurized = true,
                SeasonIds = new List<long>
                {
                    1, 2, 3, 4
                },
                WrappingIds = new List<long>
                {
                    1, 2
                }
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
                PasswordHash = "StrongPassword".ToHash(),
                Username = "jkowalski",
                DateOfBirth = DateTime.Now.AddYears(-21),
                Description = "Lubie piwo",
                Gender = "Male",
                Role = "Administrator",
                Guid = Guid.NewGuid(),
            };

            var wrapping = new Wrapping
            {
                BrandWrappings = new List<BrandWrapping>(),
                Id = 2,
                Name = "Puszka"
            };
            var brewingMethod = new BrewingMethod
            {
                BrandBrewingMethods = new List<BrandBrewingMethod>(),
                Id = 3,
                Method = "Długie"
            };
            var season = new Season
            {
                BrandSeasons = new List<BrandSeason>(),
                Id = 1,
                Name = "Jesień"
            };
            var kind = new Kind
            {
                AddedBy = user,
                AddedAt = DateTime.Now,
                Brands = new List<Brand>(),
                CountryOfOrigin = "Deutschland",
                EditedAt = DateTime.Now,
                EditedBy = user,
                Id = 15,
                IsDeleted = false,
                Name = "Porter"
            };
            //var brewery = new Brewery
            //{
            //    Id = 13,
            //    Name = "Heineken Nederland",
            //    Adress = "Street 15 15",
            //    Description = "A small brewery located in the Nederlands",
            //    AnnualProduction = 4356345,
            //    CreationDate = DateTime.Now.AddYears(-300),
            //    Owner = new BrewingGroup
            //    {
            //        Id = 6,
            //        Name = "Heineken Brewing Comapny",
            //        CreationDate = DateTime.Now.AddYears(-315),
            //        Adress = "OtherStreet 34 66",
            //        Description = "Large brewing group founded more than 300 years ago",
            //        AddedAt = DateTime.Now.AddMonths(-1),
            //        AddedBy = user,
            //        DeletionReason = "",
            //        EditedBy = user,
            //        EditedAt = DateTime.Now,
            //        Breweries = new List<Brewery>()
            //    },
            //    AddedAt = DateTime.Now,
            //    AddedBy = user,
            //    DeletionReason = "",
            //    EditedBy = user,
            //    EditedAt = DateTime.Now,
            //    BrandProductions = new List<BrandProduction>()
            //};

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            

            var brandRepository = new Mock<IBrandRepository>();
            var userRepository = new Mock<IUserRepository>();
            var enumerationRepository = new Mock<IEnumerationRepository>();
            var kindRepository = new Mock<IKindRepository>();
            var breweryRepository = new Mock<IBreweryRepository>();

            brandRepository.Setup(x => x.InsertAsync(It.IsAny<Brand>())).Returns(Task.FromResult(true));
            brandRepository.Setup(x => x.UpdateAsync(It.IsAny<Brand>())).Returns(Task.FromResult(true));
            userRepository.Setup(x => x.Get(It.IsAny<Func<ApplicationUser, bool>>())).Returns(user);
            enumerationRepository.Setup(x => x.GetWrapping(It.IsAny<Func<Wrapping, bool>>())).Returns(wrapping);
            enumerationRepository.Setup(x => x.GetBrewingMethod(It.IsAny<Func<BrewingMethod, bool>>())).Returns(brewingMethod);
            enumerationRepository.Setup(x => x.GetSeason(It.IsAny<Func<Season, bool>>())).Returns(season);
            kindRepository.Setup(x => x.Get(It.IsAny<Func<Kind, bool>>())).Returns(kind);
            //breweryRepository.Setup(x => x.Get(It.IsAny<Func<Brewery, bool>>())).Returns(brewery);

            var brandService = new BrandService(userRepository.Object, brandRepository.Object,
                enumerationRepository.Object, kindRepository.Object, breweryRepository.Object);
            var brandController = new BrandController(brandService);

            brandController.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

            var resultRaw = brandController.AddBeerBrand(newBrand).Result;
            var result = Assert.IsType<OkObjectResult>(resultRaw);
            var insertResult = Assert.IsAssignableFrom<ResponseDto<BaseModelDto>>(result.Value);

            Assert.False(insertResult.ErrorOccured);
        }
    }
}

