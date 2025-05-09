using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Update;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Threading;

namespace Constructor_API.Controllers
{
    [Route("building")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly BuildingService _buildingService;
        private readonly IAuthorizationService _authorizationService;
        //private readonly NavigationGroupService _navigationGroupService;

        public BuildingController(BuildingService buildingService, IAuthorizationService authorizationService)
        {
            _buildingService = buildingService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Добавляет здание в БД
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostBuilding([FromBody] CreateBuildingDto? buildingDto)
        {
            if (buildingDto == null) return BadRequest("Wrong input");

            var auth = await _authorizationService.AuthorizeAsync(User, buildingDto.ProjectId, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var building = await _buildingService.InsertBuilding(buildingDto, CancellationToken.None);
            return Ok(building);
        }

        //Пример заполнения:
        /// <summary>
        /// Возвращает здание по query-параметру
        /// </summary>
        /// <param name="id">ID здания, 24 символа, учитывается первым</param>
        /// <param name="name">Название здания</param>
        /// <param name="navGroupId">ID группы навигации, которой принадлежит здание, 24 символа</param>
        /// <returns>JSON-объект, представляющий здание</returns>
        /// <remarks>
        /// Примеры запроса:
        /// 
        ///     GET /BuildingController/building?id=000000000000000000000001
        ///     
        ///     GET /BuildingController/building?name=name
        ///     
        /// </remarks>
        /// <response code="200">Возвращает найденный по параметрам объект</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]

        /// <summary>
        /// Возвращает здание по ID
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBuildingById(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var building = await _buildingService.GetBuildingById(id, CancellationToken.None);
            return Ok(building);
        }

        /// <summary>
        /// Возвращает краткую информацию обо всех этажах в здании
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}/floors")]
        [Authorize]
        public async Task<IActionResult> GetFloorsByBuilding(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var floors = await _buildingService.GetSimpleFloorsByBuilding(id, CancellationToken.None);

            return Ok(floors);
        }

        /// <summary>
        /// Возвращает этаж в здании по номеру этажа
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <param name="number">Номер этажа</param> 
        /// <returns></returns>
        [HttpGet("{id}/floor/{number}")]
        [Authorize]
        public async Task<IActionResult> GetFloorInBuildingByNumber(string? id, int number)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var floor = await _buildingService.GetFloorInBuildingByNumber(id, number, CancellationToken.None);

            return Ok(floor);
        }

        /// <summary>
        /// Возвращает массив всех зданий
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllBuildings()
        {
            var buildings = await _buildingService.GetAllBuildings(CancellationToken.None);

            return Ok(buildings);
        }

        /// <summary>
        /// Удаляет здание из БД
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBuilding(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _buildingService.DeleteBuilding(id, CancellationToken.None);

            return Ok();
        }

        /// <summary>
        /// Обновляет тело здания
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <param name="buildingDto">Тело здания</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBuilding(string id, [FromBody] UpdateBuildingDto buildingDto)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _buildingService.UpdateBuilding(id, buildingDto, CancellationToken.None);

            return Ok();
        }
    }
}
