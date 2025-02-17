using ConstructorAdminAPI.Application.Result;
using ConstructorAdminAPI.Application.Services;
using ConstructorAdminAPI.Models.DTOs;
using ConstructorAdminAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ConstructorAdminAPI.Controllers
{
    [Route("floors")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly FloorService _floorService;

        public FloorController(FloorService floorService)
        {
            _floorService = floorService;
        }

        [HttpPost]
        public async Task<IActionResult> PostFloorFromBody([FromBody] CreateFloorDto? floorDto)
        {
            if (floorDto == null) return BadRequest("Wrong input");

            var res = await _floorService.InsertFloor(floorDto, CancellationToken.None);

            if (res.IsSuccessfull) return Ok();
            else
            {
                return BadRequest(res.GetErrors()[0]._message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFloors()
        {
            var floors = await _floorService.GetAllFloors(CancellationToken.None);
            return Ok(floors.Value);
        }

        [HttpGet("floors")]
        public async Task<IActionResult> GetFloorsByBuilding([FromQuery] string? building)
        {
            if (building == null) return BadRequest("Wrong input");

            var floors = await _floorService.GetFloorsByBuilding(building, CancellationToken.None);
            if (!floors.IsSuccessfull) return BadRequest(floors.GetErrors()[0]._message);

            return Ok(floors.Value);
        }

        [HttpGet("floor")]
        public async Task<IActionResult> GetFloor([FromQuery] string? id, [FromQuery] string? building, [FromQuery] int? number)
        {
            Result<Floor> res;

            if (id != null)
            {
                res = await _floorService.GetFloorById(id, CancellationToken.None);
                if (!res.IsSuccessfull) return BadRequest(res.GetErrors()[0]._message);

                return Ok(res.Value);
            }
            else if (building != null && number != null)
            {
                res = await _floorService.GetFloorByBuildingAndNumber(building, number.Value, CancellationToken.None);
                if (!res.IsSuccessfull) return BadRequest(res.GetErrors()[0]._message);

                return Ok(res.Value);
            }

            return BadRequest("Wrong input");
        }

        //[HttpGet]
        //public async Task<IActionResult> GetFloorByBuildingAndNumber([FromQuery] string? building, [FromQuery] int? number)
        //{
        //    if (building == null || number == null) return BadRequest("Wrong input");

        //    var floor = await _floorService.GetFloorByBuildingAndNumber(building, number.Value, CancellationToken.None);
        //    if (!floor.IsSuccessfull) return BadRequest(floor.GetErrors()[0]._message);

        //    return Ok(floor.Value);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetFloorById([FromQuery] string? id)
        //{
        //    if (id == null) return BadRequest("Wrong input");

        //    var floor = await _floorService.GetFloorById(id, CancellationToken.None);
        //    if (!floor.IsSuccessfull) return BadRequest(floor.GetErrors()[0]._message);

        //    return Ok(floor.Value);
        //}
    }
}
