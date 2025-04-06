using Constructor_API.Application.Result;
using System.Linq.Expressions;

namespace Constructor_API.Core.Shared.Storage
{
    public abstract class Repository<TAggregateRoot> : ReadOnlyRepository<TAggregateRoot>,
        IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        public abstract Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        public abstract Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken);
        public abstract Task RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
        public abstract Task RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
        public abstract Task UpdateAsync(Expression<Func<TAggregateRoot, bool>> predicate, TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        public abstract Task SaveChanges();
    }
}
