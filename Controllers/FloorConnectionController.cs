using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Constructor_API.Controllers
{
    [Route("floorConnection")]
    [ApiController]
    public class FloorConnectionController : ControllerBase
    {
        private readonly FloorConnectionService _floorConnectionService;
        private readonly IAuthorizationService _authorizationService;

        public FloorConnectionController(FloorConnectionService floorConnectionService, IAuthorizationService authorizationService)
        {
            _floorConnectionService = floorConnectionService;
            _authorizationService = authorizationService;
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
        /// Возвращает лестницу по ID
        /// </summary>
        /// <param name="id">ID лестницы, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetConnectionById(string id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var fc = await _floorConnectionService.GetConnectionById(id, CancellationToken.None);

            var auth = await _authorizationService.AuthorizeAsync(User, id, "FloorConnection");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            return Ok(fc);
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
        [Authorize]
        public async Task<IActionResult> GetAllConnections()
        { 
            var res = await _floorConnectionService.GetAllConnections(CancellationToken.None);

            return Ok(res);
        }
    }
}
