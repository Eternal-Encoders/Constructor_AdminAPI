using Constructor_API.Core.Repositories;
using Constructor_API.Core.Shared.S3;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Models.Entities;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Constructor_API.Application.Services
{
    public class ImageService
    {
        IS3Storage _storage;
        IImageRepository _imageRepository;
        IConfiguration _configuration;

        public ImageService(IS3Storage storage, IImageRepository imageRepository, IConfiguration configuration) 
        { 
            _storage = storage;
            _imageRepository = imageRepository;
            _configuration = configuration;
        }

        public async Task<Image> InsertImage(IFormFile file, CancellationToken cancellationToken)
        {
            if (file.Length == 0) throw new ValidationException("Empty file");

            var image = new Image();
            if (!await ImageExistsByName(file.Name))
                image.Name = file.FileName;
            else
            {
                for (var i = 1; ; i++)
                {
                    image.Name = $"{file.FileName} ({i})";
                    if (!await ImageExistsByName(file.FileName))
                        break;
                }
            }

            image.Filesize = file.Length;
            image.MimeType = file.ContentType;
            image.CreatedAt = DateTime.UtcNow;

            await _imageRepository.AddAsync(image, cancellationToken);
            await _imageRepository.SaveChanges();

            using var stream = file.OpenReadStream();
            await _storage.UploadFileAsync(
                _configuration["Bucket1"],
                image.Name,
                stream,
                CancellationToken.None);
            return image;
        }

        public async Task<Tuple<Image, MemoryStream>> GetImageByName(string fileName, CancellationToken cancellationToken)
        {
            if (await ImageExistsByName(fileName))
            {
                using MemoryStream? fileStream = await _storage.DownloadFileAsync(
                    _configuration["Bucket1"],
                    fileName, 
                    cancellationToken);

                if (fileStream == null)
                    throw new NotFoundException($"File \"{fileName}\" is not found");

                if (!fileStream.CanRead)
                    throw new Exception("Stream is not readable");

                var image = await _imageRepository.FirstAsync(i => i.Name == fileName, cancellationToken);

                return new Tuple<Image, MemoryStream>(image, fileStream);
                //var bytes = fileStream.ToArray();
            }
            else throw new NotFoundException($"File \"{fileName}\" is not found");
        }

        public async Task<Tuple<Image, MemoryStream>> GetImageById(string id, CancellationToken cancellationToken)
        {
            if (await ImageExistsById(id))
            {
                var image = await _imageRepository.FirstAsync(i => i.Id == id, cancellationToken);

                using MemoryStream? fileStream = await _storage.DownloadFileAsync(
                    _configuration["Bucket1"],
                    image.Name,
                    cancellationToken);

                if (fileStream == null)
                    throw new NotFoundException($"File \"{image.Name}\" is not found");

                if (!fileStream.CanRead)
                    throw new Exception("Stream is not readable");

                return new Tuple<Image, MemoryStream>(image, fileStream);
                //var bytes = fileStream.ToArray();
            }
            else throw new NotFoundException($"File with id \"{id}\" is not found");
        }

        public async Task<bool> ImageExistsByName(string fileName)
        {
            return await _imageRepository.CountAsync(i => i.Name == fileName, CancellationToken.None) != 0;
        }

        public async Task<bool> ImageExistsById(string id)
        {
            return await _imageRepository.CountAsync(i => i.Id == id, CancellationToken.None) != 0;
        }

        public async Task DeleteImage(string fileName, CancellationToken cancellationToken)
        {
            if (await ImageExistsByName(fileName))
            {
                await _imageRepository.RemoveAsync(i => i.Name == fileName, cancellationToken);
                await _imageRepository.SaveChanges();

                await _storage.DeleteFileAsync(_configuration["Bucket1"], fileName, cancellationToken);
            }
            else throw new NotFoundException($"File \"{fileName}\" is not found");
        }
    }
}
