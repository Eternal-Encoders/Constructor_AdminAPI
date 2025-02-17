using ConstructorAdminAPI.Application.Result;
using ConstructorAdminAPI.Application.Services;
using ConstructorAdminAPI.Models.DTOs;
using ConstructorAdminAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace ConstructorAdminAPI.Controllers
{
    [Route("buildings")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly BuildingService _buildingService;

        public BuildingController(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpPost]
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

        [HttpGet("building")]
        public async Task<IActionResult> GetBuilding([FromQuery] string? id, [FromQuery] string? name)
        {
            Result<Building> res;

            if (id != null)
            {
                res = await _buildingService.GetBuildingById(id, CancellationToken.None);
                if (!res.IsSuccessfull) return BadRequest(res.GetErrors()[0]._message);

                return Ok(res.Value);
            }
            else if (name != null) 
            {
                res = await _buildingService.GetBuildingByName(name, CancellationToken.None);
                if (!res.IsSuccessfull) return BadRequest(res.GetErrors()[0]._message);

                return Ok(res.Value);
            }

            return BadRequest("Wrong input");
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBuildings()
        {
            var buildings = await _buildingService.GetAllBuildings(CancellationToken.None);

            return Ok(buildings);
        }
    }
}
