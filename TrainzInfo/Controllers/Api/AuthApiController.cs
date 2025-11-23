using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrainzInfo.Tools;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public AuthApiController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
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

        public record RegisterDto(string Email, string Password);
        public record LoginDto(string Email, string Password);

    }
}
