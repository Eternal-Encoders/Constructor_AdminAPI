using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Update;
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
        /// Регистрирует пользователя и возвращает JWT
        /// </summary>
        /// <param name="createUserDto">Объект, состоящий из почты, имени пользователя и пароля</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null) return BadRequest("Wrong input");

            return Ok(await _userService.RegisterUser(createUserDto, CancellationToken.None));
        }

        /// <summary>
        /// Авторизирует пользователя и возвращает JWT
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
        /// Возвращает информацию о пользователе
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
        /// Удаляет пользователя
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            await _userService.DeleteUser(userId, CancellationToken.None);

            return Ok();
        }

        /// <summary>
        /// Обновляет данные о пользователе
        /// </summary>
        /// <param name="updateUserDto">Объект, состоящий из почты и имени пользователя</param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            await _userService.UpdateUser(userId, updateUserDto, CancellationToken.None);

            return Ok();
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
