using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.DTOs.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Constructor_API.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UserController(UserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null) return BadRequest("Wrong input");

            return Ok(await _userService.RegisterUser(createUserDto, CancellationToken.None));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            if (loginUserDto == null) return BadRequest("Wrong input");

            return Ok(await _userService.Login(loginUserDto, CancellationToken.None));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            return Ok(await _userService.GetUserById(userId, CancellationToken.None));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[HttpGet("projects")]
        //[Authorize]
        //public async Task<IActionResult> GetProjectsByUser()
        //{
        //    _userService.GetProjectsByUser(User.FindFirstValue(ClaimTypes.NameIdentifier), CancellationToken.None);
        //}
    }
}
