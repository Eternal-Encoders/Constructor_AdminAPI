using Constructor_API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Constructor_API.Controllers
{
    [Route("path")]
    [ApiController]
    public class PathController : ControllerBase
    {
        private readonly PathService _pathService;

        public PathController(PathService pathService)
        {
            _pathService = pathService;
        }

        /// <summary>
        /// Возвращает массив ID точек, из которых состоит маршрут между указанными точками
        /// </summary>
        /// <param name="fromId">ID начальной точки маршрута</param>
        /// <param name="toId">ID конечной точки маршрута</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPath([FromQuery] string fromId,
            [FromQuery] string toId)
        {
            var path = await _pathService.FindOptimalPath(fromId, toId);

            return Ok(path);
        }
    }
}
