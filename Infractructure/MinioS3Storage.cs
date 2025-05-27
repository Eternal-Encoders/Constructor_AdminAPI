using Constructor_API.Core.Shared.S3;
using Minio;
using Minio.DataModel.Args;

namespace Constructor_API.Infractructure
{
    public class MinioS3Storage : IS3Storage
    {
        IMinioClient _minioClient;

        public MinioS3Storage(IMinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        public async Task UploadFileAsync(string bucketName, string fileName, Stream fileStream, CancellationToken cancellationToken)
        {
            if (fileStream != null)
            {
                var args = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length);

                await _minioClient.PutObjectAsync(args);
            }
        }

        public async Task<MemoryStream?> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken)
        {
            if (!await FileExistsAsync(bucketName, fileName, cancellationToken))
                return null;

            var stream = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(s => s.CopyTo(stream));

            await _minioClient.GetObjectAsync(args);
            stream.Position = 0;
            return stream;
        }

        public async Task DeleteFileAsync(string bucketName, string fileName, CancellationToken cancellationToken)
        {
            if (await FileExistsAsync(bucketName, fileName, cancellationToken))
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName);

                await _minioClient.RemoveObjectAsync(args);
            }
        }

        public async Task<bool> FileExistsAsync(string bucketName, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var args = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName);

                await _minioClient.StatObjectAsync(args);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
