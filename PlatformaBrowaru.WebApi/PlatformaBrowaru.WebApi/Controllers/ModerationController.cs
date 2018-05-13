using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.WebApi.Controllers
{
    [Authorize]
    [Route("Moderate")]
    public class ModerationController : BaseController
    {
        private readonly IModerationService _moderationService;
        private readonly IUserService _userService;

        public ModerationController(IModerationService moderationService, IUserService userService)
        {
            _moderationService = moderationService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToModeration([FromBody] AddToModerationBindingModel addToModeration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }

            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            var result = await _moderationService.AddForModeration(userId, addToModeration);

            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

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
            var role = _userService.GetUserRole(userId);

            if (role != "Moderator" && role != "Administrator")
            {
                return Forbid();
            }

            var result = _moderationService.GetBrandsToModerate(parameters);

            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}