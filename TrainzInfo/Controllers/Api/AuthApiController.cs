using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Controllers.OldControllers;
using TrainzInfo.Data;
using TrainzInfo.Tools;
using TrainzInfo.Tools.JWT;
using TrainzInfoShared.DTO.GetDTO;

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
            Log.Init(this.ToString(), nameof(Register));
            

            Log.Wright("Register user");
            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);
            Log.Wright(result.ToString());
            if (!result.Succeeded)
            {
                Log.Wright(result.Errors.ToString());
                List<RegisterErrorsDTO> errors = result.Errors
                    .Select(x => new RegisterErrorsDTO
                    {
                        Code = x.Code,
                        Description = x.Description
                    }).ToList();
                return BadRequest(errors);
            }
            Log.Finish();
            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            Log.Init(this.ToString(), nameof(Login));
            Log.Wright("Start login");
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                Log.Wright("User not found");
                Log.Finish();
                return Unauthorized("User not found");
            }

            var passOk = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!passOk.Succeeded)
                return Unauthorized("Invalid login");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);
            Log.Wright("Generated token: " + token);
            Log.Finish();
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
            Log.Init(this.ToString(), nameof(GetAuthuser));
            

            Log.Wright("Try find user");
            var email = User.Identity?.Name;
            Log.Wright($"User email: {email}");
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                Log.Wright("User NOT FOUND");
                Log.Finish();
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

            Log.Finish();
            return Ok(dto);

        }

        public record RegisterDto(string Email, string Password);
        public record LoginDto(string Email, string Password);

    }
}
