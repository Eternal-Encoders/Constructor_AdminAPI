using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Threading;

namespace Constructor_API.Controllers
{
    [Route("BuildingController")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly BuildingService _buildingService;
        //private readonly NavigationGroupService _navigationGroupService;

        //возможно в будущем реализую
        //private static Dictionary<int, Func<StatusCodeResult>> replies = new Dictionary<int, Func<StatusCodeResult>>
        //{
        //    [404] = NotFound()
        //};

        public BuildingController(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        /// <summary>
        /// Добавляет здание в БД
        /// </summary>
        /// <param name="buildingDto">JSON объект, представляющий информацию о здании</param>
        /// <param name="navGroupId">ID группы навигации, которой принадлежит здание, 24 символа</param>
        /// <returns></returns>
        [HttpPost("building/{navGroupId}")]
        public async Task<IActionResult> PostBuilding([FromBody] CreateBuildingDto? buildingDto,
            string navGroupId)
        {
            if (buildingDto == null) return BadRequest("Wrong input");

            var res = await _buildingService.InsertBuilding(buildingDto, navGroupId, CancellationToken.None);

            if (res.IsSuccessfull) return Ok();
            else
            {
                return BadRequest(res.GetErrors()[0]._message);
            }
        }

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("building")]
        public async Task<IActionResult> GetBuilding([FromQuery] string? id, [FromQuery] string? name)
        {
            Result<Building> res;

            if (id != null)
            {
                if (ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

                res = await _buildingService.GetBuildingById(id, CancellationToken.None);
                if (!res.IsSuccessfull)
                {
                    var err = res.GetErrors()[0];
                    return err._code switch
                    {
                        404 => NotFound(res.GetErrors()[0]._message),
                        _ => BadRequest(res.GetErrors()[0]._message),
                    };
                }

                return Ok(res.Value);
            }
            else if (name != null) 
            {
                res = await _buildingService.GetBuildingByName(name, CancellationToken.None);
                if (!res.IsSuccessfull)
                {
                    var err = res.GetErrors()[0];
                    return err._code switch
                    {
                        404 => NotFound(res.GetErrors()[0]._message),
                        _ => BadRequest(res.GetErrors()[0]._message),
                    };
                }

                return Ok(res.Value);
            }

            return BadRequest("Wrong input: parameters not specified");
        }

        [HttpGet("buildings")]
        public async Task<IActionResult> GetBuildings([FromQuery] string? navGroupId)
        {
            Result<IReadOnlyList<Building>> res;

            if (navGroupId != null)
            {
                if (ObjectId.TryParse(navGroupId, out _)) return BadRequest(
                    "Wrong input: specified ID is not a valid 24 digit hex string");

                res = await _buildingService.GetBuildingsByNavGroupId(navGroupId, CancellationToken.None);
                if (!res.IsSuccessfull)
                {
                    var err = res.GetErrors()[0];
                    return err._code switch
                    {
                        404 => NotFound(res.GetErrors()[0]._message),
                        _ => BadRequest(res.GetErrors()[0]._message),
                    };
                }

                return Ok(res.Value);
            }

            return BadRequest("Wrong input: parameters not specified");
        }



        //[HttpGet]
        //public async Task<IActionResult> GetBuildingById([FromQuery] string? id)
        //{
        //    if (id == null) return BadRequest("Wrong input");

        //    var building = await _buildingService.GetBuildingById(id, CancellationToken.None);
        //    if (!building.IsSuccessfull) return BadRequest(building.GetErrors()[0]._message);

        //    return Ok(building.Value);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetBuildingByName([FromQuery] string? name)
        //{
        //    if (name == null) return BadRequest("Wrong input");

        //    var building = await _buildingService.GetBuildingByName(name, CancellationToken.None);
        //    if (!building.IsSuccessfull) return BadRequest(building.GetErrors()[0]._message);

        //    return Ok(building.Value);
        //}

        /// <summary>
        /// Возвращает массив всех зданий
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBuildings()
        {
            var buildings = await _buildingService.GetAllBuildings(CancellationToken.None);

            return Ok(buildings.Value);
        }
    }
}
