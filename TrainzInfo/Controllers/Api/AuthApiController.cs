using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
            LoggingExceptions.Init(this.ToString(), nameof(Register));
            LoggingExceptions.Start();

            LoggingExceptions.Wright("Register user");
            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await userManager.CreateAsync(user, dto.Password);
            LoggingExceptions.Wright(result.ToString());
            if (!result.Succeeded)
            {
                LoggingExceptions.Wright(result.Errors.ToString());
                List<RegisterErrorsDTO> errors = result.Errors
                    .Select(x => new RegisterErrorsDTO
                    {
                        Code = x.Code,
                        Description = x.Description
                    }).ToList();
                return BadRequest(errors);
            }
            LoggingExceptions.Finish();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            LoggingExceptions.Init(this.ToString(), nameof(Login));
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Login user");
            try
            {
                var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);

                if (!result.Succeeded)
                    return Unauthorized("Invalid login");

                //var token = jwtService.GenerateToken(dto.Email);
                LoggingExceptions.Finish();
                return Ok();

            }
            catch (System.Exception ex)
            {
                LoggingExceptions.Wright("Exception: " + ex.Message);
                LoggingExceptions.Finish();
                return BadRequest("An error occurred during login.");
            }
            finally
            {
                LoggingExceptions.Finish();
            }

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
            LoggingExceptions.Init(this.ToString(), nameof(GetAuthuser));
            LoggingExceptions.Start();
            LoggingExceptions.Wright("Get authenticated user info");
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
                LoggingExceptions.Finish();
                return Ok(userDto);
            }
            else
            {
                LoggingExceptions.Wright("User not found!");
                LoggingExceptions.Finish();
                return BadRequest();
            }

        }

        public record RegisterDto(string Email, string Password);
        public record LoginDto(string Email, string Password);

    }
}
