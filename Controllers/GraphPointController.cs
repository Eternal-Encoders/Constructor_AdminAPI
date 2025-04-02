using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Constructor_API.Controllers
{
    [Route("graphPoint")]
    [ApiController]
    public class GraphPointController : ControllerBase
    {
        private readonly GraphPointService _graphPointService;
        private readonly IAuthorizationService _authorizationService;

        public GraphPointController(GraphPointService graphPointService, IAuthorizationService authorizationService)
        {
            _graphPointService = graphPointService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Добавляет точку графа в БД
        /// </summary>
        /// <param name="graphPoint">JSON объект, представляющий информацию о точке графа</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostGraphPoint([FromBody] CreateGraphPointDto? graphPoint)
        {
            if (graphPoint == null) return BadRequest("Wrong input");

            var auth = await _authorizationService.AuthorizeAsync(User, graphPoint.FloorId, "Floor");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _graphPointService.InsertGraphPoint(graphPoint, CancellationToken.None);
            return Created();
        }

        /// <summary>
        /// Добавляет массив точек графа в БД
        /// </summary>
        /// <param name="graphPoints">Массив JSON объектов, представляющих информацию о точках графа</param>
        /// <returns></returns>
        //[HttpPost("many")]
        //public async Task<IActionResult> PostGraphPoints([FromBody] GraphPoint[]? graphPoints)
        //{
        //    if (graphPoints == null || graphPoints.Count() == 0) return BadRequest("Wrong input");

        //    await _graphPointService.InsertGraphPoints(graphPoints, CancellationToken.None);
        //    return Created();
        //}

        /// <summary>
        /// Возвращает точку графа по query-параметру
        /// </summary>
        /// <param name="id">ID точки графа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetGraphPointById(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var res = await _graphPointService.GetGraphPointById(id, CancellationToken.None);

            var auth = await _authorizationService.AuthorizeAsync(User, id, "GraphPoint");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            return Ok(res);
        }

        /// <summary>
        /// Возвращает лестницу по ID точки графа
        /// </summary>
        /// <param name="id">ID точки графа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}/stair")]
        [Authorize]
        public async Task<IActionResult> GetStairByGraphPoint(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var res = await _graphPointService.GetFloorConnectionByGraphPoint(id, CancellationToken.None);

            var auth = await _authorizationService.AuthorizeAsync(User, id, "GraphPoint");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            return Ok(res);
        }

        /// <summary>
        /// Возвращает массив всех точек графа
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllGraphPoints()
        {
            var res = await _graphPointService.GetAllGraphPoints(CancellationToken.None);

            return Ok(res);
        }
    }
}
