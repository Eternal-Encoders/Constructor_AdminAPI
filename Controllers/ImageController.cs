using Constructor_API.Application.Services;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.DTOs.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text;
using Constructor_API.Models.Entities;
using System.Net.Http.Headers;

namespace Constructor_API.Controllers
{
    [Route("image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService; 
        private readonly IAuthorizationService _authorizationService;

        public ImageController(ImageService imageService,
            IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _imageService = imageService;
        }

        /// <summary>
        /// Добавляет изображение в БД и в S3
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ValidationException("File is empty");

            await _imageService.InsertImage(file, CancellationToken.None);
            return Ok();
        }

        /// <summary>
        /// Возвращает изображение из S3
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [HttpGet("name/{fileName}")]
        [Authorize]
        public async Task<IActionResult> GetImageByName(string fileName)
        {
            if (fileName == null)
                throw new ValidationException("File name is empty");

            var tuple = await _imageService.GetImageByName(fileName, CancellationToken.None);

            var fileContent = new ByteArrayContent(tuple.Item2.ToArray());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(tuple.Item1.MimeType);

            return new FileStreamResult(fileContent.ReadAsStreamAsync().Result, tuple.Item1.MimeType)
            {
                FileDownloadName = tuple.Item1.Name
            };
        }
    }
}
