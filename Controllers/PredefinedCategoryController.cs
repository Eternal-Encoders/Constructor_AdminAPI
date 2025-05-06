using Constructor_API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Constructor_API.Controllers
{
    [Route("categories")]
    [ApiController]
    public class PredefinedCategoryController : ControllerBase
    {
        PredefinedCategoryService _service;

        public PredefinedCategoryController(PredefinedCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var res = await _service.GetPredefinedCategories(CancellationToken.None);

            return Ok(res);
        }
    }
}
