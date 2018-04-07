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
        private readonly IEmailService _emailService;

        public UsersController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
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
        [HttpPost("Test")]
        public async Task<IActionResult> Testing()
        {
            await _emailService.SendEmail("damian5996@wp.pl", "Testowanie",
                "Tutaj będzie link aktywacyjny");
            return Ok();
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

        
    }
}
