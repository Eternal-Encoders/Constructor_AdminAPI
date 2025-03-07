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
        /// <returns></returns>
        /// <remarks>
        /// Ремарка
        /// </remarks>
        [HttpPost("building")]
        public async Task<IActionResult> PostBuilding([FromBody] CreateBuildingDto? buildingDto)
        {
            if (buildingDto == null) return BadRequest("Wrong input");

            var res = await _buildingService.InsertBuilding(buildingDto, CancellationToken.None);

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
        /// <returns></returns>
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

            return Ok(buildings);
        }
    }
}
