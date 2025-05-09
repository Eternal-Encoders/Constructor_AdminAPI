using System.Linq.Expressions;

namespace Constructor_API.Core.Shared.Storage
{
    public interface IRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken);
        Task UpdateAsync(Expression<Func<TAggregateRoot, bool>> predicate, TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        //Task<TAggregateRoot> UpdateRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, 
        //    TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        Task RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
        Task RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken);
        Task SaveChanges();
    }
}
