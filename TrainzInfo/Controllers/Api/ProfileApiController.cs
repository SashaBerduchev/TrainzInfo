using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;
using TrainzInfo.Tools.DTO;

namespace TrainzInfo.Controllers.Api
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileApiController : BaseController
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ProfileApiController(UserManager<IdentityUser> userManager, ApplicationContext context)
            : base(userManager, context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("getprofile")]
        public async Task<ActionResult<IdentityUser>> GetProfile()
        {
            LoggingExceptions.Init(this.ToString(), nameof(GetProfile));
            LoggingExceptions.Start();

            LoggingExceptions.Wright("Find user");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // краще повернути 401, ніж редірект
            }

            var roles = await _userManager.GetRolesAsync(user);
            var dto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Role = roles.FirstOrDefault() // або всі ролі через string.Join(",", roles)
            };

            return Ok(dto);
        }
    }
}
