using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;

namespace PlatformaBrowaru.WebApi.Controllers
{
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("AddBeerBrand")]
        public async Task<IActionResult> AddBeerBrand([FromBody]BrandBindingModel brandBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }

            var result = await _brandService.AddBeerBrandAsync(1, brandBindingModel);
            return Ok(result);
        }
    }
}