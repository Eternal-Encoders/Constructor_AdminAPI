using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Constructor_API.Controllers
{
    [Route("project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Добавляет проект в БД
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostProject([FromBody] CreateProjectDto projectDto)
        {
            if (projectDto == null) return BadRequest("Wrong input");

            await _projectService.InsertProject(projectDto, projectDto.CreatorId, CancellationToken.None);
            return Created();
        }

        /// <summary>
        /// Возвращает проект по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var project = await _projectService.GetProjectById(id, CancellationToken.None);

            return Ok(project);
        }

        /// <summary>
        /// Возвращает все проекты
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
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
        public async Task<IActionResult> GetBuildingsByProject(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var buildings = await _projectService.GetBuildingsByProject(id, CancellationToken.None);

            return Ok(buildings);
        }

        /// <summary>
        /// Возвращает здание в проекте по его названию
        /// </summary>
        /// <param name="id">ID проекта, 24 символа</param>
        /// <param name="name">Название здания</param>
        /// <returns></returns>
        [HttpGet("{id}/building/{name}")]
        public async Task<IActionResult> GetBuildingInProjectByName(string? id, string? name)
        {
            if (id == null || name == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest(
                "Wrong input: specified ID is not a valid 24 digit hex string");

            var building = await _projectService.GetBuildingInProjectByName(id, name, CancellationToken.None);

            return Ok(building);
        }
    }
}
