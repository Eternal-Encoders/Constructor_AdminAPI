using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace Constructor_API.Controllers
{
    [Route("project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;
        private readonly IAuthorizationService _authorizationService;

        public ProjectController(ProjectService projectService, IAuthorizationService authorizationService)
        {
            _projectService = projectService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Добавляет проект в БД
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostProject([FromBody] CreateProjectDto projectDto)
        {
            if (projectDto == null) return BadRequest("Wrong input");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _projectService.InsertProject(projectDto, userId, CancellationToken.None);
            return Created();
        }

        /// <summary>
        /// Возвращает проект по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProjectById(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var project = await _projectService.GetProjectById(id, CancellationToken.None);

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            return Ok(project);
        }

        /// <summary>
        /// Возвращает все проекты
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllProjects()
        {
            var res = await _projectService.GetAllProjects(CancellationToken.None);

            return Ok(res);
        }

        /// <summary>
        /// Возвращает все здания в проекте
        /// </summary>
        /// <param name="id">ID проекта, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}/buildings")]
        [Authorize]
        public async Task<IActionResult> GetBuildingsByProject(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var buildings = await _projectService.GetBuildingsByProject(id, CancellationToken.None);

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            return Ok(buildings);
        }

        /// <summary>
        /// Возвращает здание в проекте по его названию
        /// </summary>
        /// <param name="id">ID проекта, 24 символа</param>
        /// <param name="name">Название здания</param>
        /// <returns></returns>
        [HttpGet("{id}/building/{name}")]
        [Authorize]
        public async Task<IActionResult> GetBuildingInProjectByName(string? id, string? name)
        {
            if (id == null || name == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var building = await _projectService.GetBuildingInProjectByName(id, name, CancellationToken.None);

            var auth = await _authorizationService.AuthorizeAsync(User, id, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            return Ok(building);
        }
    }
}
