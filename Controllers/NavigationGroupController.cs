using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Constructor_API.Controllers
{
    [Route("NavigationGroupController")]
    [ApiController]
    public class NavigationGroupController : ControllerBase
    {
        private readonly NavigationGroupService _navigationGroupService;

        public NavigationGroupController(NavigationGroupService navigationGroupService)
        {
            _navigationGroupService = navigationGroupService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationGroupDto"></param>
        /// <returns></returns>
        [HttpPost("navGroup")]
        public async Task<IActionResult> PostNavigationGroup([FromBody] CreateNavigationGroupDto navigationGroupDto)
        {
            if (navigationGroupDto == null) return BadRequest("Wrong input");

            var res = await _navigationGroupService.InsertNavigationGroup(navigationGroupDto, CancellationToken.None);

            if (res.IsSuccessfull) return Ok();
            else
            {
                return BadRequest(res.GetErrors()[0]._message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("navGroup")]
        public async Task<IActionResult> GetNavigationGroup([FromQuery] string? id, [FromQuery] string? name)
        {
            Result<NavigationGroup> res;

            if (id != null)
            {
                if (ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

                res = await _navigationGroupService.GetNavigationGroupById(id, CancellationToken.None);
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
            else if (name != null)
            {
                res = await _navigationGroupService.GetNavigationGroupByName(name, CancellationToken.None);
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

            return BadRequest("Wrong input: parameters not specified");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllNavigationGroups()
        {
            var res = await _navigationGroupService.GetAllNavigationGroups(CancellationToken.None);

            return Ok(res);
        }
    }
}
