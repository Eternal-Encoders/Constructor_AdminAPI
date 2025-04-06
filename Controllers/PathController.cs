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

        [HttpGet]
        public async Task<IActionResult> GetPath([FromQuery] string fromId,
            [FromQuery] string toId)
        {
            var path = await _pathService.FindOptimalPath(fromId, toId);

            return Ok(path);
        }
    }
}
