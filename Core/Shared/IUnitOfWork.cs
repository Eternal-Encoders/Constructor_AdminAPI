namespace ConstructorAdminAPI.Core.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveChanges();
    }
}
