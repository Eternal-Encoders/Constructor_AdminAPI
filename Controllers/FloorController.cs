using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Constructor_API.Controllers
{
    [Route("floor")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly FloorService _floorService;

        public FloorController(FloorService floorService)
        {
            _floorService = floorService;
        }

        /// <summary>
        /// Добавляет этаж, точки его графа и лестницы в БД
        /// </summary>
        /// <param name="floorDto">JSON объект, представляющий информацию об этаже</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostFloorFromBody([FromBody] CreateFloorDto? floorDto)
        {
            if (floorDto == null) return BadRequest("Wrong input");

            await _floorService.InsertFloor(floorDto, CancellationToken.None);

            return Created();
        }

        /// <summary>
        /// Возвращает массив всех этажей
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllFloors()
        {
            var floors = await _floorService.GetAllFloors(CancellationToken.None);
            return Ok(floors);
        }

        /// <summary>
        /// Возвращает этаж по ID
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFloorById(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var floor = await _floorService.GetFloorById(id, CancellationToken.None);

            return Ok(floor);
        }

        /// <summary>
        /// Возвращает точки графа на этаже
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}/graphPoints")]
        public async Task<IActionResult> GetGraphPointsByFloor(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var graphPoints = await _floorService.GetGraphPointsByFloor(id, CancellationToken.None);

            return Ok(graphPoints);
        }

        /// <summary>
        /// Возвращает лестницы на этаже
        /// </summary>
        /// <param name="id">ID этажа, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}/stairs")]
        public async Task<IActionResult> GetStairsByFloor(string? id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _)) return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var stairs = await _floorService.GetStairsByFloor(id, CancellationToken.None);

            return Ok(stairs);
        }
    }
}
