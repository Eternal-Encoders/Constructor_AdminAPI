using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.DTOs.Update;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;
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
        /// <param name="buildingDto">JSON объект, представляющий здание</param>
        /// <returns></returns>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// POST /building
        /// {
        ///     "project_id": "111111111111111111111111",
        ///     "name": "string",
        ///     "displayable_name": "string",
        ///     "description": "string",
        ///     "url": "string",
        ///     "latitude": 0,
        ///     "longitude": 0,
        /// }
        /// 
        /// </remarks>    
        /// <response code="200">Возвращает созданный по параметрам объект</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта, указанного в параметрах, нет в базе данных</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Building), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> PostBuilding([FromBody] CreateBuildingDto? buildingDto)
        {
            if (buildingDto == null) return BadRequest("Wrong input");

            var auth = await _authorizationService.AuthorizeAsync(User, buildingDto.ProjectId, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var building = await _buildingService.InsertBuilding(buildingDto, null, CancellationToken.None);
            return Ok(building);
        }

        /// <summary>
        /// Добавляет здание в БД, тестовая версия отправки с файлом 
        /// </summary>
        /// <param name="file">Отправляемое изображение</param>
        /// <param name="buildingDto">JSON объект, представляющий здание</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// POST /building/multipart
        /// {
        ///     "project_id": "111111111111111111111111",
        ///     "name": "string",
        ///     "displayable_name": "string",
        ///     "description": "string",
        ///     "url": "string",
        ///     "latitude": 0,
        ///     "longitude": 0,
        /// }
        /// 
        /// </remarks>
        /// <response code="200">Созданный по параметрам объект</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта, указанного в параметрах, нет в базе данных</response>
        [HttpPost("multipart")]
        [Authorize]
        [ProducesResponseType(typeof(Building), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> PostBuildingMultipart(IFormFile file, [FromForm] CreateBuildingDto buildingDto)
        {
            if (buildingDto == null) return BadRequest("Wrong input");

            var auth = await _authorizationService.AuthorizeAsync(User, buildingDto.ProjectId, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var building = await _buildingService.InsertBuilding(buildingDto, file, CancellationToken.None);
            return Ok(building);
        }

        /// <summary>
        /// Возвращает здание по ID
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// GET /building/111111111111111111111111
        /// 
        /// </remarks>
        /// <response code="200">Найденный объект</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(GetBuildingDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
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
        /// Возвращает здание по ID, тестовая версия отправки с файлом
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// GET /building/111111111111111111111111
        /// 
        /// </remarks>
        /// <response code="200">Найденный объект и изображение в формате multipart/mixed</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        [HttpGet("{id}/multipart")]
        [Authorize]
        [ProducesResponseType(typeof(GetBuildingDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetBuildingByIdMultipart(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var building = await _buildingService.GetBuildingByIdMultipart(id, CancellationToken.None);
            if (building.Item1 != null)
                return new FileStreamResult(building.Item2.ReadAsStreamAsync().Result, "multipart/mixed")
                {
                    FileDownloadName = building.Item1.Name
                };

            else return new FileStreamResult(building.Item2.ReadAsStreamAsync().Result, "multipart/mixed");
        }

        /// <summary>
        /// Возвращает краткую информацию обо всех этажах в здании
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// GET /building/111111111111111111111111/floors
        /// 
        /// </remarks>
        /// <response code="200">Найденные объекты</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        [HttpGet("{id}/floors")]
        [Authorize]
        [ProducesResponseType(typeof(GetSimpleFloorDto[]), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
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
        /// <remarks>
        /// Пример запроса:
        /// 
        /// GET /building/111111111111111111111111/floor/1
        /// 
        /// </remarks>
        /// <response code="200">Найденный объект</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        [HttpGet("{id}/floor/{number}")]
        [Authorize]
        [ProducesResponseType(typeof(Floor), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
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
        /// <remarks>
        /// Пример запроса:
        /// 
        /// GET /building/all
        /// 
        /// </remarks>
        /// <response code="200">Найденные объекты</response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        [HttpGet("all")]
        [Authorize]
        [ProducesResponseType(typeof(Building[]), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetAllBuildings()
        {
            var buildings = await _buildingService.GetAllBuildings(CancellationToken.None);

            return Ok(buildings);
        }

        /// <summary>
        /// Удаляет здание из БД
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// DELETE /building/111111111111111111111111
        /// 
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта нет в базе данных</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
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
        /// <param name="id">ID здания, 24 символа</param>
        /// <param name="buildingDto">JSON объект, представляющий обновляемое здание, может содержать пустые поля</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// PATCH /building/111111111111111111111111
        /// {
        ///     "project_id": "111111111111111111111111",
        ///     "name": "string",
        ///     "displayable_name": "string",
        ///     "description": "string",
        ///     "url": "string",
        ///     "latitude": 0,
        ///     "longitude": 0,
        ///     "defaultFloorId": "string"
        /// }
        /// 
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта или указанных в параметрах объектов нет в базе данных</response>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpdateBuilding(string id, [FromBody] UpdateBuildingDto buildingDto)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _buildingService.UpdateBuilding(id, buildingDto, null, CancellationToken.None);

            return Ok();
        }

        /// <summary>
        /// Обновляет тело здания, тестовая версия с отправкой файла
        /// </summary>
        /// <param name="id">ID здания, 24 символа</param>
        /// <param name="file">Отправляемое изображение</param>
        /// <param name="buildingDto">JSON объект, представляющий обновляемое здание, может содержать пустые поля</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// PATCH /building/111111111111111111111111/multipart
        /// {
        ///     "project_id": "111111111111111111111111",
        ///     "name": "string",
        ///     "displayable_name": "string",
        ///     "description": "string",
        ///     "url": "string",
        ///     "latitude": 0,
        ///     "longitude": 0,
        ///     "defaultFloorId": "string"
        /// }
        /// 
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если объекта или указанных в параметрах объектов нет в базе данных</response>
        [HttpPatch("{id}/multipart")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpdateBuildingMultipart(string id, IFormFile file, [FromForm] UpdateBuildingDto buildingDto)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _buildingService.UpdateBuilding(id, buildingDto, file, CancellationToken.None);

            return Ok();
        }
    }
}
