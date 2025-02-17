using ConstructorAdminAPI.Application.Result;
using System.Linq.Expressions;

namespace ConstructorAdminAPI.Core.Shared.Storage
{
    public abstract class Repository<TAggregateRoot> : ReadOnlyRepository<TAggregateRoot>,
        IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        public abstract Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        public abstract Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken);
        public abstract Task RemoveByIdAsync(string id, CancellationToken cancellationToken);
        public abstract Task RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
        public abstract Task RemoveRangeByIdsAsync(string[] Ids, CancellationToken cancellationToken);
        public abstract Task UpdateAsync(string id, TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        public abstract Task SaveChanges();
    }
}
