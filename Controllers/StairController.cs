using ConstructorAdminAPI.Application.Result;
using ConstructorAdminAPI.Application.Services;
using ConstructorAdminAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ConstructorAdminAPI.Controllers
{
    [Route("stairs")]
    [ApiController]
    public class StairController : ControllerBase
    {
        private readonly StairService _stairService;

        public StairController(StairService stairService)
        {
            _stairService = stairService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetStairById([FromQuery] string? id)
        //{
        //    if (id == null) return BadRequest("Wrong input");

        //    var stair = await _stairService.GetStairById(id, CancellationToken.None);
        //    if (!stair.IsSuccessfull) return BadRequest(stair.GetErrors()[0]._message);

        //    return Ok(stair.Value);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetStairByGraphPoint([FromQuery] string? graphPointId)
        //{
        //    if (graphPointId == null) return BadRequest("Wrong input");

        //    var stair = await _stairService.GetStairByGraphPoint(graphPointId, CancellationToken.None);
        //    if (!stair.IsSuccessfull) return BadRequest(stair.GetErrors()[0]._message);

        //    return Ok(stair.Value);
        //}

        [HttpGet("stair")]
        public async Task<IActionResult> GetStair([FromQuery] string? id, [FromQuery] string? graphPointId)
        {
            Result<Stair> res; 

            if (id != null)
            {
                res = await _stairService.GetStairById(id, CancellationToken.None);
                if (!res.IsSuccessfull) return BadRequest(res.GetErrors()[0]._message);

                return Ok(res.Value);
            }
            else if (graphPointId != null)
            {
                res = await _stairService.GetStairByGraphPoint(graphPointId, CancellationToken.None);
                if (!res.IsSuccessfull) return BadRequest(res.GetErrors()[0]._message);

                return Ok(res.Value);
            }
            else return BadRequest("Wrong input");
        }

        [HttpGet("stairs")]
        public async Task<IActionResult> GetStairsByBuilding([FromQuery] string? buildingName)
        {
            if (buildingName == null) return BadRequest("Wrong input");

            var stair = await _stairService.GetStairsByBuilding(buildingName, CancellationToken.None);
            if (!stair.IsSuccessfull) return BadRequest(stair.GetErrors()[0]._message);

            return Ok(stair.Value);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStairs()
        { 
            var stair = await _stairService.GetAllStairs(CancellationToken.None);
            if (!stair.IsSuccessfull) return BadRequest(stair.GetErrors()[0]._message);

            return Ok(stair.Value);
        }
    }
}
