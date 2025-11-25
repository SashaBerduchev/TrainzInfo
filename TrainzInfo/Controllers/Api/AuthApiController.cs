using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;
using TrainzInfo.Tools.JWT;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;

        public AuthApiController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ApplicationContext context, JwtService jwtService)
            : base(userManager, context)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._jwtService = jwtService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            LoggingExceptions.Init(this.ToString(), nameof(Register));
            LoggingExceptions.Start();

            LoggingExceptions.Wright("Register user");
            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("User not found");

            var passOk = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!passOk.Succeeded)
                return Unauthorized("Invalid login");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(token);

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }


        [HttpGet("getauthuser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserDto>> GetAuthuser()
        {
            LoggingExceptions.Init(this.ToString(), nameof(GetAuthuser));
            LoggingExceptions.Start();

            LoggingExceptions.Wright("Try find user");
            var email = User.Identity?.Name;
            LoggingExceptions.Wright($"User email: {email}");
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                LoggingExceptions.Wright("User NOT FOUND");
                LoggingExceptions.Finish();
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var dto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Role = roles.FirstOrDefault() ?? "User",
                IsAuthenticated = true
            };

            LoggingExceptions.Finish();
            return Ok(dto);

        }

        public record RegisterDto(string Email, string Password);
        public record LoginDto(string Email, string Password);

    }
}
