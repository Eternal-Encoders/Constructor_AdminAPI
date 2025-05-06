namespace Constructor_API.Core.Shared.S3
{
    public interface IS3Storage
    {
        Task UploadFileAsync(string bucketName, string fileName, Stream fileStream, CancellationToken cancellationToken);
        Task<MemoryStream?> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken);
        Task DeleteFileAsync(string bucketName, string fileName, CancellationToken cancellationToken);
        Task<bool> FileExistsAsync(string bucketName, string name, CancellationToken cancellationToken);
    }
}
