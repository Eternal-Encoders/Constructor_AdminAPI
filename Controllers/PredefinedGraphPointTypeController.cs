using Constructor_API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Constructor_API.Controllers
{
    [Route("graphPointTypes")]
    [ApiController]
    public class PredefinedGraphPointTypeController : ControllerBase
    {
        PredefinedGraphPointTypeService _service;

        public PredefinedGraphPointTypeController(PredefinedGraphPointTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllGraphPointTypes()
        {
            var res = await _service.GetPredefinedTypes(CancellationToken.None);

            return Ok(res);
        }
    }
}
