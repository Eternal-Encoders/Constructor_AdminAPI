using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Constructor_API.Infractructure.Repositories
{
    public sealed class FloorsTransitionMongoRepository : MongoRepository<FloorsTransition>, IFloorsTransitionRepository
    {
        readonly IMongoCollection<GraphPoint> graphPointCollection;
        public FloorsTransitionMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {
            graphPointCollection = dbContext.GetCollection<GraphPoint>(typeof(GraphPoint).Name);
            //Удаление перехода
            //Обновление точек графов с id перехода - удаление поля или установка null
        }

        public override async Task RemoveAsync(Expression<Func<FloorsTransition, bool>> predicate, CancellationToken cancellationToken)
        {
            var transition = await base.FirstOrDefaultAsync(predicate, cancellationToken);
            if (transition != null)
            {
                var updateSettings = new BsonDocument("$set", new BsonDocument("TransitionId", BsonNull.Value));
                await base.AddCommand(async (IClientSessionHandle s) => await graphPointCollection.UpdateManyAsync(
                    gp => gp.TransitionId == transition.Id, updateSettings));

                await base.RemoveAsync(predicate, cancellationToken);
            }
        }
    }
}
