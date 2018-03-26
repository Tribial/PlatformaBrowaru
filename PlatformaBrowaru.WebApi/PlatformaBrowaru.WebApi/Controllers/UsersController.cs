using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlatformaBrowaru.Services.Services.Interfaces;
using PlatformaBrowaru.Share.BindingModels;

namespace PlatformaBrowaru.WebApi.Controllers
{
    [Route("Users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginBindingModel loginModel)
        {
            var result = await _userService.LoginAsync(loginModel);
            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }
    }
}
