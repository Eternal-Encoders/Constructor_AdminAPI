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
        /// Тест
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [HttpGet("multipart/name/{fileName}")]
        [Authorize]
        public async Task<IActionResult> GetImageByNameMultipart(string fileName)
        {
            if (fileName == null)
                throw new ValidationException("File name is empty");

            var tuple = await _imageService.GetImageByName(fileName, CancellationToken.None);
            var multipartContent = new MultipartContent("mixed"); 

            var jsonContent = new StringContent(JsonSerializer.Serialize(tuple.Item1), Encoding.UTF8, "application/json");
            multipartContent.Add(jsonContent);

            var fileContent = new ByteArrayContent(tuple.Item2.ToArray());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(tuple.Item1.MimeType);
            multipartContent.Add(fileContent);

            return new FileStreamResult(multipartContent.ReadAsStreamAsync().Result, "multipart/mixed")
            {
                FileDownloadName = tuple.Item1.Name
            };
            //return null;
        }

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

        /// <summary>
        /// Тест
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [HttpGet("multipart/id/{id}")]
        [Authorize]
        public async Task<IActionResult> GetImageByIdMultipart(string id)
        {
            if (id == null)
                throw new ValidationException("File id is empty");

            var tuple = await _imageService.GetImageById(id, CancellationToken.None);
            var multipartContent = new MultipartContent("mixed");

            var jsonContent = new StringContent(JsonSerializer.Serialize(tuple.Item1), Encoding.UTF8, "application/json");
            multipartContent.Add(jsonContent);

            var fileContent = new ByteArrayContent(tuple.Item2.ToArray());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(tuple.Item1.MimeType);
            multipartContent.Add(fileContent);

            return new FileStreamResult(multipartContent.ReadAsStreamAsync().Result, "multipart/mixed")
            {
                FileDownloadName = tuple.Item1.Name
            };
            //return null;
        }
    }
}
