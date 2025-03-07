using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Constructor_API.Controllers
{
    [Route("FloorController")]
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
        [HttpPost("floor")]
        public async Task<IActionResult> PostFloorFromBody([FromBody] CreateFloorDto? floorDto)
        {
            if (floorDto == null) return BadRequest("Wrong input");

            var res = await _floorService.InsertFloor(floorDto, CancellationToken.None);

            if (res.IsSuccessfull) return Ok();
            else
            {
                return StatusCode(res.GetErrors()[0]._code, res.GetErrors()[0]._message);
            }
        }

        /// <summary>
        /// Возвращает массив всех этажей
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllFloors()
        {
            var floors = await _floorService.GetAllFloors(CancellationToken.None);
            return Ok(floors.Value);
        }

        /// <summary>
        /// Возвращает массив этажей по query-параметру
        /// </summary>
        /// <param name="building">Название здания</param>
        /// <returns></returns>
        [HttpGet("floors")]
        public async Task<IActionResult> GetFloorsByBuilding([FromQuery] string? building)
        {
            if (building == null) return BadRequest("Wrong input");

            var res = await _floorService.GetFloorsByBuilding(building, CancellationToken.None);
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

        /// <summary>
        /// Возвращает этаж по query-параметрам
        /// </summary>
        /// <param name="id">ID этажа, 24 символа, учитывается первым</param>
        /// <param name="building">Название здания, учитывается вместе с номером этажа</param>
        /// <param name="number">Номер этажа, учитывается вместе с названием здания</param>
        /// <returns></returns>
        [HttpGet("floor")]
        public async Task<IActionResult> GetFloor([FromQuery] string? id, [FromQuery] string? building, [FromQuery] int? number)
        {
            Result<Floor> res;

            if (id != null)
            {
                res = await _floorService.GetFloorById(id, CancellationToken.None);
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
            else if (building != null && number != null)
            {
                res = await _floorService.GetFloorByBuildingAndNumber(building, number.Value, CancellationToken.None);
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

            return BadRequest("Wrong input");
        }

        //[HttpGet]
        //public async Task<IActionResult> GetFloorByBuildingAndNumber([FromQuery] string? building, [FromQuery] int? number)
        //{
        //    if (building == null || number == null) return BadRequest("Wrong input");

        //    var floor = await _floorService.GetFloorByBuildingAndNumber(building, number.Value, CancellationToken.None);
        //    if (!floor.IsSuccessfull) return BadRequest(floor.GetErrors()[0]._message);

        //    return Ok(floor.Value);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetFloorById([FromQuery] string? id)
        //{
        //    if (id == null) return BadRequest("Wrong input");

        //    var floor = await _floorService.GetFloorById(id, CancellationToken.None);
        //    if (!floor.IsSuccessfull) return BadRequest(floor.GetErrors()[0]._message);

        //    return Ok(floor.Value);
        //}
    }
}
