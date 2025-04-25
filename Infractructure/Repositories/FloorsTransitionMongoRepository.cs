using Constructor_API.Core.Repositories;
using Constructor_API.Models.Entities;

namespace Constructor_API.Infractructure.Repositories
{
    public sealed class FloorsTransitionMongoRepository : MongoRepository<FloorsTransition>, IFloorsTransitionRepository
    {
        public FloorsTransitionMongoRepository(MongoDBContext dbContext) : base(dbContext, false)
        {

        }
    }
}
