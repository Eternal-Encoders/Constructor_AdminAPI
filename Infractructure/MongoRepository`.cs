﻿using Constructor_API.Core.Shared;
using Constructor_API.Core.Shared.Storage;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace Constructor_API.Infractructure
{
    public class MongoRepository<TAggregateRoot>: Repository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        private readonly MongoDBContext _dbContext;
        protected readonly IMongoCollection<TAggregateRoot> DbCollection;
        public MongoRepository(MongoDBContext dbContext, bool isReadOnly)
        {
            _dbContext = dbContext;
            DbCollection = _dbContext.GetCollection<TAggregateRoot>(typeof(TAggregateRoot).Name);
            ReadOnly = isReadOnly;
        }

        public async Task AddCommand(Func<IClientSessionHandle, Task> func)
        {
            if (!ReadOnly)
            {
                await _dbContext.AddCommand(func);
            }
        }

        public override async Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(async (IClientSessionHandle s) => await DbCollection.InsertOneAsync(aggregateRoot));
            }
        }

        public override async Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(async (IClientSessionHandle s) => await DbCollection.InsertManyAsync(aggregateRoots));
            }
        }

        public override async Task RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(async (IClientSessionHandle s) => await DbCollection.DeleteOneAsync(predicate));
            }
        }

        public override async Task RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(async (IClientSessionHandle s) => await DbCollection.DeleteManyAsync(predicate));
            }
        }

        public override async Task UpdateAsync(Expression<Func<TAggregateRoot, bool>> predicate, TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(async (IClientSessionHandle s) => await DbCollection.ReplaceOneAsync(predicate, aggregateRoot));
            }
        }

        public async Task PartialyUpdateAsync(Expression<Func<TAggregateRoot, bool>> predicate, BsonDocument document, CancellationToken cancellationToken)
        {
            if (!ReadOnly)
            {
                _dbContext.AddCommand(async (IClientSessionHandle s) => await DbCollection.UpdateOneAsync(predicate, document));
            }
        }

        public override async Task<TAggregateRoot> FirstAsync(CancellationToken cancellationToken)
        {
            return (await DbCollection.FindAsync(Builders<TAggregateRoot>.Filter.Empty)).First();
        }

        public override async Task<TAggregateRoot> FirstAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return (await DbCollection.FindAsync(predicate)).First();
        }

        public override async Task<TAggregateRoot?> FirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            var data = await DbCollection.FindAsync(Builders<TAggregateRoot>.Filter.Empty);
            return data.FirstOrDefault();
        }

        public override async Task<TAggregateRoot?> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            var data = await DbCollection.FindAsync(predicate);
            return data.FirstOrDefault();
        }

        public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            var elements = await DbCollection.FindAsync(predicate);
            return elements == null ? [] : elements.ToList();   
        }

        public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(CancellationToken cancellationToken)
        {
            var elements = await DbCollection.FindAsync(Builders<TAggregateRoot>.Filter.Empty);
            return elements == null ? [] : elements.ToList();
        }

        public override async Task<long> LongCountAsync(CancellationToken cancellationToken)
        {
            return await DbCollection.CountDocumentsAsync(Builders<TAggregateRoot>.Filter.Empty);
        }

        public override async Task<long> LongCountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await DbCollection.CountDocumentsAsync(predicate);
        }

        public override async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return Convert.ToInt32(await DbCollection.CountDocumentsAsync(Builders<TAggregateRoot>.Filter.Empty));
        }

        public override async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return Convert.ToInt32(await DbCollection.CountDocumentsAsync(predicate));
        }

        public override async Task SaveChanges()
        {
            await _dbContext.SaveChanges();
        }
    }
}
