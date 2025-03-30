using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Constructor_API.Controllers
{
    [Route("floorConnection")]
    [ApiController]
    public class FloorConnectionController : ControllerBase
    {
        private readonly FloorConnectionService _floorConnectionService;

        public FloorConnectionController(FloorConnectionService floorConnectionService)
        {
            _floorConnectionService = floorConnectionService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetStairByGraphPoint([FromQuery] string? graphPointId)
        //{
        //    if (graphPointId == null) return BadRequest("Wrong input");

        //    var stair = await _stairService.GetStairByGraphPoint(graphPointId, CancellationToken.None);
        //    if (!stair.IsSuccessfull) return BadRequest(stair.GetErrors()[0]._message);

        //    return Ok(stair.Value);
        //}

        /// <summary>
        /// Возвращает лестницу по query-параметру
        /// </summary>
        /// <param name="id">ID лестницы, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{stairId}")]
        public async Task<IActionResult> GetStairById(string id)
        {
            if (id != null)
            {
                if (!ObjectId.TryParse(id, out _))
                    return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

                return Ok(await _floorConnectionService.GetConnectionById(id, CancellationToken.None));
            }
            
            else return BadRequest("Wrong input");
        }

        /// <summary>
        /// Возвращает массив лестниц по query-параметру
        /// </summary>
        /// <param name="buildingName">Название здания</param>
        /// <returns></returns>
        //[HttpGet("stairs")]
        //public async Task<IActionResult> GetStairsByBuilding([FromQuery] string? buildingName)
        //{
        //    if (buildingName == null) return BadRequest("Wrong input");

        //    var res = await _stairService.GetStairsByBuilding(buildingName, CancellationToken.None);
        //    if (!res.IsSuccessfull)
        //    {
        //        var err = res.GetErrors()[0];
        //        return err._code switch
        //        {
        //            404 => NotFound(res.GetErrors()[0]._message),
        //            _ => BadRequest(res.GetErrors()[0]._message),
        //        };
        //    }

        //    return Ok(res.Value);
        //}

        /// <summary>
        /// Возвращает массив всех лестниц
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStairs()
        { 
            var res = await _floorConnectionService.GetAllConnections(CancellationToken.None);

            return Ok(res);
        }
    }
}
