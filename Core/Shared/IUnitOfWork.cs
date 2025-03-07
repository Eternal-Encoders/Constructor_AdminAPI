namespace Constructor_API.Core.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveChanges();
    }
}
