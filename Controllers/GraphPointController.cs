using ConstructorAdminAPI.Application.Result;
using ConstructorAdminAPI.Application.Services;
using ConstructorAdminAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConstructorAdminAPI.Controllers
{
    [Route("graphPoints")]
    [ApiController]
    public class GraphPointController : ControllerBase
    {
        private readonly GraphPointService _graphPointService;

        public GraphPointController(GraphPointService graphPointService)
        {
            _graphPointService = graphPointService;
        }

        [HttpPost]
        public async Task<IActionResult> PostGraphPoint([FromBody] GraphPoint? graphPoint)
        {
            if (graphPoint == null) return BadRequest("Wrong input");

            var res = await _graphPointService.InsertGraphPoint(graphPoint, CancellationToken.None);

            if (res.IsSuccessfull) return Ok();
            else
            {
                return BadRequest(res.GetErrors()[0]._message);
            }
        }

        [HttpGet("graphPoint")]
        public async Task<IActionResult> GetGraphPointById([FromQuery] string? id)
        {
            if (id == null) return BadRequest("Wrong input");

            var graphPoint = await _graphPointService.GetGraphPointById(id, CancellationToken.None);
            if (!graphPoint.IsSuccessfull) return BadRequest(graphPoint.GetErrors()[0]._message);

            return Ok(graphPoint.Value);
        }

        [HttpGet("graphPoints")]
        public async Task<IActionResult> GetGraphPoints([FromQuery] string? buildingName, [FromQuery] int? floorNum)
        {
            Result<IReadOnlyList<GraphPoint>> res;

            if (buildingName != null)
            {
                if (floorNum != null)
                {
                    res = await _graphPointService.GetGraphPointsByFloor(buildingName, floorNum.Value, CancellationToken.None);
                    if (!res.IsSuccessfull) return BadRequest(res.GetErrors()[0]._message);

                    return Ok(res.Value);
                }
                else
                {
                    res = await _graphPointService.GetGraphPointsByBuilding(buildingName, CancellationToken.None);
                    if (!res.IsSuccessfull) return BadRequest(res.GetErrors()[0]._message);

                    return Ok(res.Value);
                }
            }

            return BadRequest("Wrong input");
        }

        //[HttpGet]
        //public async Task<IActionResult> GetGraphPointsByBuilding([FromQuery] string? buildingName)
        //{
        //    if (buildingName == null) return BadRequest("Wrong input");

        //    var graphPoints = await _graphPointService.GetGraphPointsByBuilding(buildingName, CancellationToken.None);
        //    if (!graphPoints.IsSuccessfull) return BadRequest(graphPoints.GetErrors()[0]._message);

        //    return Ok(graphPoints.Value);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetGraphPointsByFloor([FromQuery] string? buildingName, [FromQuery] int? floorNum)
        //{
        //    if (buildingName == null || floorNum == null) return BadRequest("Wrong input");

        //    var graphPoints = await _graphPointService.GetGraphPointsByFloor(buildingName, floorNum.Value, CancellationToken.None);
        //    if (!graphPoints.IsSuccessfull) return BadRequest(graphPoints.GetErrors()[0]._message);

        //    return Ok(graphPoints.Value);
        //}

        [HttpGet("all")]
        public async Task<IActionResult> GetAllGraphPoints()
        {
            var graphPoints = await _graphPointService.GetAllGraphPoints(CancellationToken.None);
            if (!graphPoints.IsSuccessfull) return BadRequest(graphPoints.GetErrors()[0]._message);

            return Ok(graphPoints.Value);
        }
    }
}
