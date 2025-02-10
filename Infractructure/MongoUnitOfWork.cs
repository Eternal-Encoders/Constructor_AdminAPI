using ConstructorAdminAPI.Core.Shared;

namespace ConstructorAdminAPI.Infractructure
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoDBContext _context;

        public MongoUnitOfWork(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChanges()
        {
            var changeAmount = await _context.SaveChanges();
            return changeAmount;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
