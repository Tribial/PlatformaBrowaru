using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;

namespace PlatformaBrowaru.WebApi.Controllers
{
    [Route("Users")]
    //[Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginBindingModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelStateErrors());
            }

            var result = await _userService.LoginAsync(loginModel);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody]RegisterBindingModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.Register(registerModel);

            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            var result = await _userService.LogoutAsync(userId);

            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
