using ConstructorAdminAPI.Application.Result;
using ConstructorAdminAPI.Core.Shared;
using ConstructorAdminAPI.Core.Shared.Storage;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace ConstructorAdminAPI.Infractructure
{
    public class MongoRepository<TAggregateRoot>: Repository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        private readonly MongoDBContext _dbContext;
        private IMongoCollection<TAggregateRoot> DbSet;
        public MongoRepository(MongoDBContext dbContext, bool isReadOnly)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
            ReadOnly = isReadOnly;
        }

        public override async Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(() => DbSet.InsertOneAsync(aggregateRoot));
            }
        }

        public override async Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(() => DbSet.InsertManyAsync(aggregateRoots));
            }
        }

        public override async Task RemoveByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(() => DbSet.DeleteOneAsync(Builders<TAggregateRoot>.Filter.Eq("_id", new ObjectId(id))));
            }
        }

        public override async Task RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(() => DbSet.DeleteManyAsync(predicate));
            }
        }

        public override async Task RemoveRangeByIdsAsync(string[] Ids, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {

            }
            throw new NotImplementedException();
        }

        public override async Task UpdateAsync(string id, TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TAggregateRoot>.Filter.Eq("_id", new ObjectId(id)), aggregateRoot));
            }
        }

        public override async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return Convert.ToInt32(await DbSet.CountDocumentsAsync(Builders<TAggregateRoot>.Filter.Empty));
        }

        public override async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return Convert.ToInt32(await DbSet.CountDocumentsAsync(predicate));
        }

        public override async Task<TAggregateRoot?> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task<TAggregateRoot> FirstAsync(CancellationToken cancellationToken)
        {
            return (await DbSet.FindAsync(Builders<TAggregateRoot>.Filter.Empty)).First();
        }

        public override async Task<TAggregateRoot> FirstAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return (await DbSet.FindAsync(predicate)).First();
        }

        public override async Task<TAggregateRoot?> FirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            var data = await DbSet.FindAsync(Builders<TAggregateRoot>.Filter.Empty);
            return data.FirstOrDefault();
        }

        public override async Task<TAggregateRoot?> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            var data = await DbSet.FindAsync(predicate);
            return data.FirstOrDefault();
        }

        public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            var elements = await DbSet.FindAsync(predicate);
            return elements == null ? [] : elements.ToList();   
        }

        public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(CancellationToken cancellationToken)
        {
            var elements = await DbSet.FindAsync(Builders<TAggregateRoot>.Filter.Empty);
            return elements == null ? [] : elements.ToList();
        }

        public override async Task<long> LongCountAsync(CancellationToken cancellationToken)
        {
            return await DbSet.CountDocumentsAsync(Builders<TAggregateRoot>.Filter.Empty);
        }

        public override async Task<long> LongCountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await DbSet.CountDocumentsAsync(predicate);
        }

        public override async Task SaveChanges()
        {
            await _dbContext.SaveChanges();
        }
    }
}
