using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;
using PlatformaBrowaru.Share.Models;
using PlatformaBrowaru.Share.ModelsDto;

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

        [AllowAnonymous]
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
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterBindingModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAsync(registerModel);

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

        [HttpGet("{id}")]
        public IActionResult GetUser(long id)
        {
            var result = _userService.GetUser(id);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Activate/{guid}")]
        public IActionResult ActivateUser(Guid guid)
        {
            var result = _userService.ActivateUser(guid);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("List")]
        public IActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteUser([FromBody]string password)
        {
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var userId = Convert.ToInt64(rawUserId);

            var result = _userService.DeleteUser(userId, password);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("ChangeEmail")]
        public IActionResult ChangeEmail([FromBody]ChangeEmailBindingModel changeEmailModel)
        {
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var userId = Convert.ToInt64(rawUserId);

            var result = _userService.ChangeEmail(userId, changeEmailModel);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("ChangePassword")]
        public IActionResult ChangePassword([FromBody]ChangePasswordBindingModel changePasswordModel)
        {
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var userId = Convert.ToInt64(rawUserId);

            var result = _userService.ChangePassword(userId, changePasswordModel);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditUserProfile(long id, [FromBody] UserProfileBindingModel userProfile)
        {
            var rawUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userId = Convert.ToInt64(rawUserId);

            ResponseDto<BaseModelDto> result;
            if (userId == id)
            {
                result = await _userService.EditUserProfile(id, userProfile);
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
    }
}
