using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : BaseController
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public AuthApiController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ApplicationContext context)
            : base(userManager, context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            LoggingExceptions.LogInit(this.ToString(), nameof(Register));
            LoggingExceptions.LogStart();

            LoggingExceptions.LogWright("Register user");
            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await userManager.CreateAsync(user, dto.Password);
            LoggingExceptions.LogWright(result.ToString());
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            LoggingExceptions.LogFinish();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid login");

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }


        [HttpGet("getauthuser")]
        public async Task<ActionResult<UserDto>> GetAuthuser()
        {
            LoggingExceptions.LogInit(this.ToString(), nameof(GetAuthuser));
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("Get authenticated user info");
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                UserDto userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = (await _userManager.GetRolesAsync(user)).Count > 0 ? (await _userManager.GetRolesAsync(user))[0] : "User",
                    IsAuthenticated = User.Identity.IsAuthenticated
                };
                LoggingExceptions.LogFinish();
                return Ok(userDto);
            }
            else
            {
                LoggingExceptions.LogWright("User not found!");
                LoggingExceptions.LogFinish();
                return BadRequest();
            }

        }

        public record RegisterDto(string Email, string Password);
        public record LoginDto(string Email, string Password);

    }
}
