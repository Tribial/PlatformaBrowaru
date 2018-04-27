using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.WebApi.Controllers
{
    [Route("Brands")]
    //[Authorize]
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("AddBrand")]
        public async Task<IActionResult> AddBeerBrand([FromBody] BrandBindingModel brandBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }

            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            var result = await _brandService.AddBeerBrandAsync(userId, brandBindingModel);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditBeerBrand(long id, [FromBody] EditBrandBindingModel beerBrand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }

            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            ResponseDto<BaseModelDto> result;
            if (userId == 2) //Trzeba zmienić na role później
            {
                result = await _brandService.EditBeerBrandAsync(userId, id, beerBrand);
            }
            else
            {
                return Unauthorized();
            }

            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{beerBrandId}/Details")]
        public IActionResult GetBeerBrand(long beerBrandId)
        {
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            var result = _brandService.GetBeerBrand(beerBrandId, userId);
            return Ok(result);
        }

        [HttpPatch("{beerBrandId}/Delete")]
        public async Task<IActionResult> DeleteBeerBrandAsync(long beerBrandId, [FromBody] DeleteBeerBrandBindingModel deleteBrandModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            var result = await _brandService.DeleteBeerBrandAsync(beerBrandId, userId, deleteBrandModel);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetBrands([FromQuery] BrandSearchBindingModel parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }

            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            var result = _brandService.GetBrands(userId, parameters);

            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("{beerBrandId}/Rate")]
        public async Task<IActionResult> AddRatingAsync(long beerBrandId, [FromBody] AddRatingBindingModel addRatingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            var result = await _brandService.AddOrEditRatingAsync(beerBrandId, userId, addRatingModel);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{beerBrandId}/Rate")]
        public async Task<IActionResult> DeleteRatingAsync(long beerBrandId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            var result = await _brandService.DeleteRatingAsync(beerBrandId, userId);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}