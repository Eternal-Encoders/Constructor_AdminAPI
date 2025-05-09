using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Update;
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

            var project = await _projectService.InsertProject(projectDto, userId, CancellationToken.None);
            return Ok(project);
        }

        /// <summary>
        /// Возвращает информацию о проекте по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProjectById(string? id)
        {
            var auth = await _authorizationService.AuthorizeAsync(User, id, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var project = await _projectService.GetProjectById(id, userId, CancellationToken.None);

            return Ok(project);
        }

        /// <summary>
        /// Возвращает все проекты, тестовый запрос
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
        /// Возвращает краткую информацию обо всех зданиях в проекте
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
        /// Удаляет проект из БД
        /// </summary>
        /// <param name="id">Id проекта, 24 символа</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProject(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");
            var auth = await _authorizationService.AuthorizeAsync(User, id, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _projectService.DeleteProject(id, userId, CancellationToken.None);

            return Ok();
        }

        /// <summary>
        /// Обновляет поля проекта
        /// </summary>
        /// <param name="id">Id проекта, 24 символа</param>
        /// <param name="projectDto">Тело проекта</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectDto projectDto, string id)
        {
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");
            var auth = await _authorizationService.AuthorizeAsync(User, id, "Project");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _projectService.UpdateProject(id, projectDto, CancellationToken.None);

            return Ok();
        }
    }
}
