using Constructor_API.Application.Result;
using Constructor_API.Application.Services;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Constructor_API.Controllers
{
    [Route("floorsTransition")]
    [ApiController]
    public class FloorsTransitionController : ControllerBase
    {
        private readonly FloorsTransitionService _floorsTransitionService;
        private readonly IAuthorizationService _authorizationService;

        public FloorsTransitionController(FloorsTransitionService floorsTransitionService, IAuthorizationService authorizationService)
        {
            _floorsTransitionService = floorsTransitionService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Добавляет переход между этажами в БД
        /// </summary>
        /// <param name="transitionDto">Объект, представляющий собой переход</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertTransition([FromBody] CreateFloorsTransitionDto transitionDto)
        {
            if (transitionDto == null) return BadRequest("Wrong input");

            var auth = await _authorizationService.AuthorizeAsync(User, transitionDto.BuildingId, "Building");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            await _floorsTransitionService.InsertTransition(transitionDto, CancellationToken.None);

            return Created();
        }

        /// <summary>
        /// Возвращает переход по ID
        /// </summary>
        /// <param name="id">ID перехода, 24 символа</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTransitionById(string id)
        {
            if (id == null) return BadRequest("Wrong input");
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Wrong input: specified ID is not a valid 24 digit hex string");

            var auth = await _authorizationService.AuthorizeAsync(User, id, "FloorsTransition");
            if (!auth.Succeeded)
            {
                return Forbid();
            }

            var fc = await _floorsTransitionService.GetTransitionById(id, CancellationToken.None);


            return Ok(fc);
        }

        /// <summary>
        /// Возвращает массив переходов по query-параметру
        /// </summary>
        /// <param name="buildingName">Название здания</param>
        /// <returns></returns>
        //[HttpGet("stairs")]
        //public async Task<IActionResult> GetStairsByBuilding([FromQuery] string? buildingName)
        //{
        //    if (buildingName == null) return BadRequest("Wrong input");

        //    var res = await _stairService.GetStairsByBuilding(buildingName, CancellationToken.None);
        //    if (!res.IsSuccessfull)
        //    {
        //        var err = res.GetErrors()[0];
        //        return err._code switch
        //        {
        //            404 => NotFound(res.GetErrors()[0]._message),
        //            _ => BadRequest(res.GetErrors()[0]._message),
        //        };
        //    }

        //    return Ok(res.Value);
        //}

        /// <summary>
        /// Возвращает массив всех переходов
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllConnections()
        { 
            var res = await _floorsTransitionService.GetAllTransitions(CancellationToken.None);

            return Ok(res);
        }
    }
}
