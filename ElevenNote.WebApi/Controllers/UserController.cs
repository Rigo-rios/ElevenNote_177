using ElevenNote.Models.Reponses;
using ElevenNote.Models.User;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace ElevenNote.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _userService.RegisterUserAsync(model);
            if(registerResult)
            {
                TextResponse response = new("User was registered.");
                return Ok(response);
            }

            return BadRequest(new TextResponse("User could not be registered"));
        }

        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int userId)
        {
            UserDetail? detail = await _userService.GetUserIdAsync(userId);

            if(detail is null)
            {
                return NotFound();
            }
            return Ok(detail);
        }
    }
}
