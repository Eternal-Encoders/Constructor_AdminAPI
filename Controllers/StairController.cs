using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Constructor_API.Controllers
{
    [Route("StairController")]
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

        /// <summary>
        /// Возвращает лестницу по query-параметру
        /// </summary>
        /// <param name="id">ID лестницы, 24 символа, учитывается первым</param>
        /// <param name="graphPointId">ID точки графа, соответствующей лестнице, 24 символа</param>
        /// <returns></returns>
        [HttpGet("stair")]
        public async Task<IActionResult> GetStair([FromQuery] string? id, [FromQuery] string? graphPointId)
        {
            Result<Stair> res; 

            if (id != null)
            {
                res = await _stairService.GetStairById(id, CancellationToken.None);
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
            else if (graphPointId != null)
            {
                res = await _stairService.GetStairByGraphPoint(graphPointId, CancellationToken.None);
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
            else return BadRequest("Wrong input");
        }

        /// <summary>
        /// Возвращает массив лестниц по query-параметру
        /// </summary>
        /// <param name="buildingName">Название здания</param>
        /// <returns></returns>
        [HttpGet("stairs")]
        public async Task<IActionResult> GetStairsByBuilding([FromQuery] string? buildingName)
        {
            if (buildingName == null) return BadRequest("Wrong input");

            var res = await _stairService.GetStairsByBuilding(buildingName, CancellationToken.None);
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

        /// <summary>
        /// Возвращает массив всех лестниц
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStairs()
        { 
            var res = await _stairService.GetAllStairs(CancellationToken.None);
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
    }
}
