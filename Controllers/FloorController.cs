using ConstructorAdminAPI.Application.Services;
using ConstructorAdminAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConstructorAdminAPI.Controllers
{
    [Route("floor")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly FloorService _floorService;

        public FloorController(FloorService floorService)
        {
            _floorService = floorService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GEtAllFloors()
        {
            var floors = await _floorService.GetAllFloors(CancellationToken.None);
            return Ok(floors.Value);
        }

        [HttpPost]
        public async Task<IActionResult> PostFloorFromBody([FromBody] CreateFloorDto floorDto)
        {
            var res = await _floorService.InsertFloor(floorDto, CancellationToken.None);

            if (res.IsSuccessfull) return Ok();
            else
            {
                return BadRequest(res.GetErrors()[0]._message);
            }
        }
    }
}
