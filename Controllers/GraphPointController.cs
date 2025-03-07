using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Constructor_API.Controllers
{
    [Route("GraphPointController")]
    [ApiController]
    public class GraphPointController : ControllerBase
    {
        private readonly GraphPointService _graphPointService;

        public GraphPointController(GraphPointService graphPointService)
        {
            _graphPointService = graphPointService;
        }

        /// <summary>
        /// Добавляет точку графа в БД
        /// </summary>
        /// <param name="graphPoint">JSON объект, представляющий информацию о точке графа</param>
        /// <returns></returns>
        [HttpPost("graphPoint")]
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

        /// <summary>
        /// Добавляет массив точек графа в БД
        /// </summary>
        /// <param name="graphPoints">Массив JSON объектов, представляющих информацию о точках графа</param>
        /// <returns></returns>
        [HttpPost("graphPoints")]
        public async Task<IActionResult> PostGraphPoints([FromBody] GraphPoint[]? graphPoints)
        {
            if (graphPoints == null || graphPoints.Count() == 0) return BadRequest("Wrong input");

            var res = await _graphPointService.InsertGraphPoints(graphPoints, CancellationToken.None);

            if (res.IsSuccessfull) return Ok();
            else
            {
                return BadRequest(res.GetErrors()[0]._message);
            }
        }

        /// <summary>
        /// Возвращает точку графа по query-параметру
        /// </summary>
        /// <param name="id">ID точки графа, 24 символа, учитывается первым</param>
        /// <returns></returns>
        [HttpGet("graphPoint")]
        public async Task<IActionResult> GetGraphPointById([FromQuery] string? id)
        {
            if (id == null) return BadRequest("Wrong input");

            var res = await _graphPointService.GetGraphPointById(id, CancellationToken.None);
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
        /// Возвращает массив точек графа по query-параметру
        /// </summary>
        /// <param name="buildingName">Название здания</param>
        /// <param name="floorNum">Номер этажа, учитывается вместе с названием здания</param>
        /// <returns></returns>
        [HttpGet("graphPoints")]
        public async Task<IActionResult> GetGraphPoints([FromQuery] string? buildingName, [FromQuery] int? floorNum)
        {
            Result<IReadOnlyList<GraphPoint>> res;

            if (buildingName != null)
            {
                if (floorNum != null)
                {
                    res = await _graphPointService.GetGraphPointsByFloor(buildingName, floorNum.Value, CancellationToken.None);
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
                else
                {
                    res = await _graphPointService.GetGraphPointsByBuilding(buildingName, CancellationToken.None);
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

        /// <summary>
        /// Возвращает массив всех точек графа
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllGraphPoints()
        {
            var res = await _graphPointService.GetAllGraphPoints(CancellationToken.None);
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
