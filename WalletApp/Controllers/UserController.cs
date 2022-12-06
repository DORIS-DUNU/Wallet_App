using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Abstractions.Services;
using WalletApp.Models.DTO;

namespace WalletApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _logger = logger;
            _userService = userService;
        }

         [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO user)
        {

            var register = await _userService.Register(user);

            if (register != null)
                return Ok(register);

            _logger.LogInformation("Registration failed!");
            return BadRequest();    
        }
       

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _userService.LoginAsync(model);
            if (token != null) return Ok(token);

            _logger.LogInformation("Log in failed!");
            return Problem();
        }



    }
}
