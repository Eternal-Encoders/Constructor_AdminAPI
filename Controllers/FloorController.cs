using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Update;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Constructor_API.Controllers
{
    [Route("floor")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly FloorService _floorService;
        private readonly IAuthorizationService _authorizationService;

        public FloorController(FloorService floorService, IAuthorizationService authorizationService)
        {
            _floorService = floorService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Добавляет этаж в БД
        /// </summary>
        /// <param name="floorDto">JSON объект, представляющий этаж</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// POST /floor
        /// {
        ///       "index": 0,
        ///       "name": "string",
        ///       "building_id": "111111111111111111111111"
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
        [ProducesResponseType(typeof(Floor), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> PostFloor([FromBody] CreateFloorDto? floorDto)
        {
            if (floorDto == null) return BadRequest("Wrong input");

            var auth = await _authorizationService.AuthorizeAsync(User, floorDto.BuildingId, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var floor = await _floorService.InsertFloor(floorDto, CancellationToken.None);

            return Ok(floor);
        }

        /// <summary>
        /// Создание карты этажа на основе схемы
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/schema")]
        [Authorize]
        public async Task<IActionResult> PostSchemaFloor(IFormFile file)
        {
            return Ok();
        }

        /// <summary>
        /// Возвращает массив всех этажей
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        [ProducesResponseType(typeof(Floor[]), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetAllFloors()
        {
            var floors = await _floorService.GetAllFloors(CancellationToken.None);

            return Ok(floors);
        }

        /// <summary>
        /// Возвращает этаж по ID с точками маршрутного графа
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetFloorById(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Floor");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var floor = await _floorService.GetFloorByIdWithGraphPoints(id, CancellationToken.None);

            return Ok(floor);
        }

        /// <summary>
        /// Возвращает этаж по ID с точками маршрутного графа, тестовая версия отправки с файлом
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}/multipart")]
        [Authorize]
        public async Task<IActionResult> GetFloorByIdMultipart(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Floor");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var floor = await _floorService.GetFloorByIdWithGraphPointsMultipart(id, CancellationToken.None);

            if (floor.Item1 != null)
                return new FileStreamResult(floor.Item2.ReadAsStreamAsync().Result, "multipart/mixed")
                {
                    FileDownloadName = floor.Item1.Name
                };

            else return new FileStreamResult(floor.Item2.ReadAsStreamAsync().Result, "multipart/mixed");
        }

        /// <summary>
        /// Возвращает точки графа на этаже
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}/graphPoints")]
        [Authorize]
        public async Task<IActionResult> GetGraphPointsByFloor(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Floor");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var graphPoints = await _floorService.GetGraphPointsByFloor(id, CancellationToken.None);

            return Ok(graphPoints);
        }

        /// <summary>
        /// Возвращает лестницы на этаже
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}/stairs")]
        [Authorize]
        public async Task<IActionResult> GetStairsByFloor(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Floor");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var stairs = await _floorService.GetStairsByFloor(id, CancellationToken.None);

            return Ok(stairs);
        }

        /// <summary>
        /// Удаляет этаж из БД
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFloor(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Floor");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _floorService.DeleteFloor(id, CancellationToken.None);

            return Ok();
        }

        /// <summary>
        /// Обновляет тело этажа
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <param name="floorDto">JSON объект, представляющий обновляемый этаж, может содержать пустые поля</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// PATCH /floor/111111111111111111111111
        /// {
        ///     "index": 0,
        ///     "name": "string",
        ///     "building_id": "111111111111111111111111",
        ///     "width": 0,
        ///     "height": 0,
        ///     "decorations": [
        ///     {
        ///         "points": [
        ///         {
        ///             "x": 0,
        ///             "y": 0
        ///         }
        ///         ],
        ///         "stroke": "string",
        ///         "fill": "string"
        ///     }
        ///     ],
        ///     "graph_points": [
        ///     {
        ///         "id": "111111111111111111111111",
        ///         "x": 0,
        ///         "y": 0,
        ///         "links": [
        ///         "string"
        ///         ],
        ///         "types": [
        ///         "string"
        ///         ],
        ///         "name": "string",
        ///         "synonyms": [
        ///         "string"
        ///         ],
        ///         "time": [
        ///         {
        ///             "is_day_off": true,
        ///             "from": "string",
        ///             "to": "string"
        ///         }
        ///         ],
        ///         "description": "string",
        ///         "route_active": true,
        ///         "search_active": true,
        ///         "transition_id": "string"
        ///     }
        ///     ],
        ///     "rooms": [
        ///     {
        ///         "id": "111111111111111111111111",
        ///         "points": [
        ///         {
        ///             "x": 0,
        ///             "y": 0
        ///         }
        ///         ],
        ///         "fill": "string",
        ///         "stroke": "string",
        ///         "floor_id": "string",
        ///         "children": [
        ///         {
        ///             "type": "string",
        ///             "value": "string",
        ///             "x": 0,
        ///             "y": 0
        ///         }
        ///         ],
        ///         "passages": [
        ///         {
        ///             "id": "111111111111111111111111",
        ///             "x": 0,
        ///             "y": 0,
        ///             "width": 0,
        ///             "height": 0,
        ///             "fill": "string",
        ///         }
        ///         ]
        ///     }]
        /// }
        /// 
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">Если неправильно указаны параметры запроса</response>
        /// <response code="401">Если пользователь не авторизован</response>
        /// <response code="403">Если у пользователя нет доступа к объекту</response>
        /// <response code="404">Если указанного объекта или объекта, указанного в параметрах, нет в базе данных</response>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpdateFloor(string id, [FromBody] UpdateFloorDto floorDto)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Floor");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _floorService.UpdateFloor(id, floorDto, null, CancellationToken.None);

            return Ok();
        }

        /// <summary>
        /// Обновляет тело этажа
        /// </summary>
        /// <param name="id">D этажа, 24 символа</param>
        /// <param name="floorDto">Тело этажа</param>
        /// <returns></returns>
        [HttpPatch("{id}/multipart")]
        [Authorize]
        public async Task<IActionResult> UpdateFloorMultipart(string id, IFormFile file, [FromForm] UpdateFloorDto floorDto)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Floor");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _floorService.UpdateFloor(id, floorDto, file, CancellationToken.None);

            return Ok();
        }

        /// <summary>
        /// Обновляет тело этажа
        /// </summary>
        /// <param name="id">D этажа, 24 символа</param>
        /// <param name="floorDto">Тело этажа</param>
        /// <returns></returns>
        //[HttpPatch("{id}")]
        //[Authorize]
        //public async Task<IActionResult> SaveFloor(string id, [FromBody] UpdateFloorDto floorDto)
        //{
        //    if (id == null) return BadRequest("Wrong input");
        //    if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

        //    var auth = await _authorizationService.AuthorizeAsync(User, id, "Floor");
        //    if (!auth.Succeeded)
        //    {
        //        return Forbid();
        //    }

        //    await _floorService.UpdateFloor(id, floorDto, CancellationToken.None);

        //    return Ok();
        //}
    }
}
