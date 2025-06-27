using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.DTOs.Update;
using Constructor_API.Models.Entities;
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

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Регистрирует пользователя и возвращает JWT
        /// </summary>
        /// <param name="createUserDto">JSON объект, состоящий из почты, имени пользователя и пароля</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// POST /user/register
        /// {
        ///      "nickname": "string",
        ///      "email": "string",
        ///      "password": "stringst"
        /// }
        /// 
        /// </remarks>   
        /// <response code="200">Возвращает созданный по параметрам объект</response>
        /// <response code="400">Если неправильно указаны параметры запроса или уникальные данные повторяются</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null) return BadRequest("Wrong input");

            return Ok(await _userService.RegisterUser(createUserDto, CancellationToken.None));
        }

        /// <summary>
        /// Авторизирует пользователя и возвращает JWT
        /// </summary>
        /// <param name="loginUserDto">JSON объект, состоящий из почты и пароля</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// POST /user/login
        /// {
        ///      "email": "string",
        ///      "password": "stringst"
        /// }
        /// 
        /// </remarks>   
        /// <response code="200">Возвращает созданный по параметрам объект</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            if (loginUserDto == null) return BadRequest("Wrong input");

            return Ok(await _userService.Login(loginUserDto, CancellationToken.None));
        }

        /// <summary>
        /// Возвращает информацию о пользователе, необходим токен
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// GET /user/info
        /// 
        /// </remarks>
        /// <response code="200">Возвращает найденный объект</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        [HttpGet("info")]
        [Authorize]
        [ProducesResponseType(typeof(GetUserDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            return Ok(await _userService.GetUserById(userId, CancellationToken.None));
        }

        /// <summary>
        /// Возвращает список краткой информации о проектах пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("projects")]
        [Authorize]
        public async Task<IActionResult> GetProjectsByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            return Ok(await _userService.GetProjectsByUser(userId, CancellationToken.None));
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
    }
}
